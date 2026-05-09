/* global React */
const { useState: useStateT, useMemo: useMemoT, useRef: useRefT, useEffect: useEffectT } = React;

// ============================================================
// ScheduleTimeline — dark, instrument-grade
// Horizontal SVG, one row per Device-Group.
// Drag to create; conflict band when overlapping confirmed.
// ============================================================

function ScheduleTimeline({ initialDayOffset = 0, onNewReservation, tweaks = {} }) {
  const [view, setView] = useStateT("day"); // "day" | "week"
  const [dayOffset, setDayOffset] = useStateT(initialDayOffset);
  const [reservations, setReservations] = useStateT(() => JSON.parse(JSON.stringify(window.Reservations)));
  const [selected, setSelected] = useStateT(null);

  const groups = window.DeviceGroups.filter(g => g.status === "Active" || reservations.some(r => r.groupId === g.id));
  // Render Active first then Inactive
  const orderedGroups = [
    ...groups.filter(g => g.status === "Active"),
    ...groups.filter(g => g.status !== "Active"),
  ];

  // Geometry
  const ROW_H = 48;
  const HOUR_W = view === "day" ? (tweaks.hourW || 70) : Math.max(20, Math.round((tweaks.hourW || 70) * 0.4));
  const START_H = 6, END_H = 22;
  const HOURS_PER_DAY = END_H - START_H;
  const DAYS_VISIBLE = view === "day" ? 1 : 7;

  // Reservations visible
  const fromOffset = view === "day" ? dayOffset : (dayOffset - ((window.dateForOffset(dayOffset).getDay() + 6) % 7)); // align to Monday
  const toOffset = fromOffset + DAYS_VISIBLE - 1;
  const visible = reservations.filter(r => r.dayOffset >= fromOffset && r.dayOffset <= toOffset);

  const totalW = HOUR_W * HOURS_PER_DAY * DAYS_VISIBLE;
  const totalH = ROW_H * orderedGroups.length;

  // Drag-to-create
  const [drag, setDrag] = useStateT(null); // {groupId, startX, currentX, dayOffset}
  const svgRef = useRefT(null);

  const xToTime = (x) => {
    // returns {dayOffset, hourFloat}
    const totalHourSlot = x / HOUR_W;
    const dayIdx = Math.floor(totalHourSlot / HOURS_PER_DAY);
    const hourFloat = (totalHourSlot - dayIdx * HOURS_PER_DAY) + START_H;
    return { dayOffset: fromOffset + dayIdx, hourFloat };
  };

  const handleMouseDown = (e, groupId) => {
    if (e.button !== 0) return;
    const r = svgRef.current.getBoundingClientRect();
    const x = e.clientX - r.left + svgRef.current.parentElement.scrollLeft;
    setDrag({ groupId, startX: x, currentX: x });
  };
  const handleMouseMove = (e) => {
    if (!drag) return;
    const r = svgRef.current.getBoundingClientRect();
    const x = e.clientX - r.left + svgRef.current.parentElement.scrollLeft;
    setDrag({ ...drag, currentX: x });
  };
  const handleMouseUp = () => {
    if (!drag) return;
    const ax = Math.min(drag.startX, drag.currentX);
    const bx = Math.max(drag.startX, drag.currentX);
    if (bx - ax > 8) {
      const a = xToTime(ax), b = xToTime(bx);
      // snap to 15-min and clamp same-day for simplicity
      const snap = (h) => Math.round(h * 4) / 4;
      const startHour = Math.max(START_H, snap(a.hourFloat));
      const endHour = Math.min(END_H, Math.max(startHour + 0.25, snap(b.hourFloat)));
      const newR = {
        id: "r" + Math.random().toString(36).slice(2, 7),
        groupId: drag.groupId, testGroupId: window.TestGroups[0].id,
        dayOffset: a.dayOffset, startHour, endHour, status: "Pending", notes: "",
      };
      setReservations(prev => [...prev, newR]);
      setSelected(newR.id);
    }
    setDrag(null);
  };

  // Conflict bands while dragging: where drag overlaps a Confirmed reservation on same group/test-group
  const conflictBands = useMemoT(() => {
    if (!drag) return [];
    const ax = Math.min(drag.startX, drag.currentX);
    const bx = Math.max(drag.startX, drag.currentX);
    const a = xToTime(ax), b = xToTime(bx);
    const startHour = Math.max(START_H, a.hourFloat);
    const endHour = Math.min(END_H, b.hourFloat);
    const out = [];
    reservations.forEach(r => {
      if (r.dayOffset !== a.dayOffset) return;
      if (r.status !== "Confirmed") return;
      const overlap = startHour < r.endHour && endHour > r.startHour;
      if (!overlap) return;
      const sameGroup = r.groupId === drag.groupId;
      if (sameGroup) out.push({ rule: "R10", r });
    });
    return out;
  }, [drag, reservations]);

  // Layout helpers per reservation
  const dayIndex = (off) => off - fromOffset;
  const xForResv = (r) => dayIndex(r.dayOffset) * HOURS_PER_DAY * HOUR_W + (r.startHour - START_H) * HOUR_W;
  const wForResv = (r) => (r.endHour - r.startHour) * HOUR_W;

  // Date label
  const headerDate = useMemoT(() => {
    if (view === "day") {
      const d = window.dateForOffset(dayOffset);
      return d.toLocaleDateString("en", { weekday: "long", month: "short", day: "numeric", year: "numeric" });
    }
    const a = window.dateForOffset(fromOffset);
    const b = window.dateForOffset(toOffset);
    return `${a.toLocaleDateString("en", { month: "short", day: "numeric" })} – ${b.toLocaleDateString("en", { month: "short", day: "numeric", year: "numeric" })}`;
  }, [view, dayOffset, fromOffset, toOffset]);

  const sel = reservations.find(r => r.id === selected);

  return (
    <div className="rs-timeline-screen">
      <div className="rs-timeline-head">
        <div className="rs-row">
          <div>
            <div className="rs-screen-eyebrow">07 · Schedule Timeline</div>
            <div className="rs-screen-title" style={{ fontSize: 18 }}>{headerDate}</div>
          </div>
        </div>
        <div className="rs-timeline-controls">
          <div className="rs-timeline-legend">
            <span><span className="swatch" style={{ background: "transparent", border: "1.5px solid var(--rs-ok)" }} />Pending</span>
            <span><span className="swatch" style={{ background: "var(--rs-ok)" }} />Confirmed</span>
            <span><span className="swatch" style={{ background: "var(--rs-bad-soft)", textDecoration: "line-through" }} />Cancelled</span>
            <span><span className="swatch" style={{ background: "var(--rs-off-soft)", border: "1px solid var(--rs-off-line)" }} />Completed</span>
            <span style={{ paddingLeft: 12, borderLeft: "1px solid var(--rs-line)" }}>
              <span className="swatch" style={{ background: "var(--rs-bad)" }} />Conflict
            </span>
          </div>
          <div className="rs-toggle">
            <button className={view === "day" ? "is-active" : ""} onClick={() => setView("day")}>DAY</button>
            <button className={view === "week" ? "is-active" : ""} onClick={() => setView("week")}>WEEK</button>
          </div>
          <button className="rs-btn rs-btn--sm" onClick={() => setDayOffset(o => o - (view === "day" ? 1 : 7))}>‹</button>
          <button className="rs-btn rs-btn--sm" onClick={() => setDayOffset(0)}>Today</button>
          <button className="rs-btn rs-btn--sm" onClick={() => setDayOffset(o => o + (view === "day" ? 1 : 7))}>›</button>
          <button className="rs-btn rs-btn--primary rs-btn--sm" onClick={onNewReservation}>+ Reservation</button>
        </div>
      </div>

      <div className="rs-timeline-wrap">
        {/* Left rail */}
        <div className="rs-timeline-rail">
          <div className="rs-timeline-rail-header">Device-Group</div>
          {orderedGroups.map(g => {
            const count = g.deviceIds.length;
            return (
              <div key={g.id} className="rs-timeline-rail-row" style={{ height: ROW_H }}>
                <span className="name">
                  <span className={`rs-dot rs-dot--${g.status === "Active" ? "ok" : "off"}`} style={{ width: 6, height: 6, marginRight: 6 }} />
                  {g.name}
                </span>
                <span className="badge">{count}d</span>
              </div>
            );
          })}
        </div>

        {/* Canvas */}
        <div className="rs-timeline-canvas"
          onMouseMove={handleMouseMove} onMouseUp={handleMouseUp} onMouseLeave={handleMouseUp}>
          <svg ref={svgRef} width={totalW} height={40 + totalH} style={{ display: "block" }}>
            {/* Header band */}
            <rect x="0" y="0" width={totalW} height="40" fill="var(--rs-surface)" />
            <line x1="0" y1="40" x2={totalW} y2="40" stroke="var(--rs-line)" />

            {/* Day separators (week view) */}
            {view === "week" && Array.from({ length: DAYS_VISIBLE }, (_, i) => {
              const x = i * HOURS_PER_DAY * HOUR_W;
              const d = window.dateForOffset(fromOffset + i);
              const isToday = (fromOffset + i) === 0;
              return (
                <g key={i}>
                  <line x1={x} y1="0" x2={x} y2={40 + totalH} stroke="var(--rs-line-2)" strokeWidth={i === 0 ? 0 : 1} />
                  {isToday && (
                    <rect x={x} y="40" width={HOURS_PER_DAY * HOUR_W} height={totalH} fill="var(--rs-accent-soft)" opacity="0.18" />
                  )}
                  <text x={x + 8} y="16" fontFamily="IBM Plex Mono, ui-monospace, monospace"
                    fontSize="10" fill="var(--rs-ink-3)" letterSpacing="0.06em">
                    {d.toLocaleDateString("en", { weekday: "short" }).toUpperCase()} {String(d.getDate()).padStart(2, "0")}
                  </text>
                </g>
              );
            })}

            {/* Hour ticks */}
            {Array.from({ length: HOURS_PER_DAY * DAYS_VISIBLE + 1 }, (_, i) => {
              const x = i * HOUR_W;
              const hourOfDay = (i % HOURS_PER_DAY) + START_H;
              const major = (hourOfDay % (view === "day" ? 1 : 4) === 0);
              return (
                <g key={i}>
                  <line x1={x} y1="40" x2={x} y2={40 + totalH}
                    stroke={major ? "var(--rs-line-2)" : "var(--rs-line)"}
                    strokeWidth={major ? 1 : 0.5}
                    strokeDasharray={major ? "" : "2 4"} />
                  {(view === "day" || major) && i < HOURS_PER_DAY * DAYS_VISIBLE && (
                    <text x={x + 4} y={view === "week" ? 32 : 28}
                      fontFamily="IBM Plex Mono, ui-monospace, monospace"
                      fontSize="10" fill="var(--rs-ink-4)">
                      {String(hourOfDay).padStart(2, "0")}:00
                    </text>
                  )}
                </g>
              );
            })}

            {/* Row stripes + group hit areas */}
            {orderedGroups.map((g, ri) => {
              const y = 40 + ri * ROW_H;
              return (
                <g key={g.id}>
                  <rect x="0" y={y} width={totalW} height={ROW_H}
                    fill={ri % 2 === 0 ? "transparent" : "var(--rs-surface)"} opacity="0.5" />
                  <line x1="0" y1={y + ROW_H} x2={totalW} y2={y + ROW_H} stroke="var(--rs-line)" />
                  {/* hit area */}
                  <rect x="0" y={y} width={totalW} height={ROW_H} fill="transparent"
                    style={{ cursor: g.status === "Active" ? "crosshair" : "not-allowed" }}
                    onMouseDown={(e) => g.status === "Active" && handleMouseDown(e, g.id)} />
                </g>
              );
            })}

            {/* Now-line if today is in view */}
            {fromOffset <= 0 && toOffset >= 0 && (() => {
              const todayIdx = -fromOffset;
              const nowH = 11.25; // simulated "now" (11:15 AM)
              const x = todayIdx * HOURS_PER_DAY * HOUR_W + (nowH - START_H) * HOUR_W;
              return (
                <g>
                  <line x1={x} y1="40" x2={x} y2={40 + totalH} stroke="var(--rs-bad)" strokeWidth="1" strokeDasharray="3 3" />
                  <rect x={x - 26} y="40" width="52" height="14" fill="var(--rs-bad)" rx="2" />
                  <text x={x} y="51" textAnchor="middle"
                    fontFamily="IBM Plex Mono, ui-monospace, monospace"
                    fontSize="9" fill="white" letterSpacing="0.06em">NOW · 11:15</text>
                </g>
              );
            })()}

            {/* Reservations */}
            {visible.map(r => {
              const ri = orderedGroups.findIndex(g => g.id === r.groupId);
              if (ri < 0) return null;
              const x = xForResv(r);
              const y = 40 + ri * ROW_H + 6;
              const w = wForResv(r);
              const h = ROW_H - 12;
              const tg = window.TestGroups.find(t => t.id === r.testGroupId);
              const isSel = selected === r.id;
              return (
                <ReservationBlock key={r.id}
                  r={r} x={x} y={y} w={w} h={h} tg={tg} isSel={isSel}
                  confirmedFilled={tweaks.confirmedFilled !== false}
                  onClick={(e) => { e.stopPropagation(); setSelected(r.id); }}
                />
              );
            })}

            {/* Drag preview */}
            {drag && (() => {
              const ri = orderedGroups.findIndex(g => g.id === drag.groupId);
              const y = 40 + ri * ROW_H + 6;
              const ax = Math.min(drag.startX, drag.currentX);
              const bx = Math.max(drag.startX, drag.currentX);
              const conflict = conflictBands.length > 0;
              const a = xToTime(ax), b = xToTime(bx);
              return (
                <g>
                  <rect x={ax} y={y} width={Math.max(2, bx - ax)} height={ROW_H - 12} rx="3"
                    fill={conflict ? "var(--rs-bad-soft)" : "var(--rs-accent-soft)"}
                    stroke={conflict ? "var(--rs-bad)" : "var(--rs-accent)"}
                    strokeWidth="1.5" strokeDasharray="4 3" />
                  <text x={ax + 8} y={y + 18}
                    fontFamily="IBM Plex Mono, ui-monospace, monospace"
                    fontSize="10" fill={conflict ? "var(--rs-bad)" : "var(--rs-accent)"}>
                    {window.fmtHourMono(a.hourFloat)} – {window.fmtHourMono(b.hourFloat)}
                  </text>
                  {conflict && (
                    <text x={ax + 8} y={y + 32}
                      fontFamily="IBM Plex Mono, ui-monospace, monospace"
                      fontSize="9" fill="var(--rs-bad)">⚠ R10 conflict · {conflictBands[0].r.id}</text>
                  )}
                </g>
              );
            })()}
          </svg>
        </div>
      </div>

      {/* Selected reservation drawer */}
      {sel && (
        <div style={{
          position: "absolute", right: 24, bottom: 24,
          width: 360, background: "var(--rs-surface-2)", border: "1px solid var(--rs-line)",
          borderRadius: 3, padding: 16, color: "var(--rs-ink)",
          boxShadow: "0 8px 24px rgba(0,0,0,0.35)",
        }}>
          <div className="rs-row" style={{ justifyContent: "space-between", marginBottom: 8 }}>
            <span className="rs-mono-sm">RESERVATION · {sel.id}</span>
            <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => setSelected(null)}>×</button>
          </div>
          <div style={{ fontWeight: 500, marginBottom: 4 }}>{window.DeviceGroups.find(g => g.id === sel.groupId)?.name}</div>
          <div className="rs-mono-sm">
            {window.dateForOffset(sel.dayOffset).toLocaleDateString("en", { weekday: "short", month: "short", day: "numeric" })}
            {" · "}{window.fmtHourMono(sel.startHour)}–{window.fmtHourMono(sel.endHour)}
          </div>
          <div style={{ marginTop: 8, marginBottom: 10 }}>
            <Pill status={sel.status} kind="resv" />
            <span style={{ marginLeft: 8 }} className="rs-mono-sm">
              {window.TestGroups.find(t => t.id === sel.testGroupId)?.name}
            </span>
          </div>
          {sel.notes && <div style={{ fontSize: 12, color: "var(--rs-ink-3)" }}>{sel.notes}</div>}
          <div className="rs-row" style={{ marginTop: 12, gap: 6 }}>
            {sel.status === "Pending" && (
              <button className="rs-btn rs-btn--primary rs-btn--sm"
                onClick={() => setReservations(prev => prev.map(r => r.id === sel.id ? { ...r, status: "Confirmed" } : r))}>
                Confirm
              </button>
            )}
            <button className="rs-btn rs-btn--sm rs-btn--danger"
              onClick={() => setReservations(prev => prev.map(r => r.id === sel.id ? { ...r, status: "Cancelled" } : r))}>
              Cancel
            </button>
          </div>
        </div>
      )}
    </div>
  );
}

function ReservationBlock({ r, x, y, w, h, tg, isSel, onClick, confirmedFilled = true }) {
  const cls = window.resvStatusToClass(r.status);
  const isPending = r.status === "Pending";
  const isCancelled = r.status === "Cancelled";
  const isCompleted = r.status === "Completed";
  const isConfirmed = r.status === "Confirmed";

  const fill = isConfirmed ? (confirmedFilled ? "var(--rs-ok)" : "var(--rs-ok-soft)")
    : isPending ? "transparent"
    : isCancelled ? "var(--rs-bad-soft)"
    : "var(--rs-off-soft)";
  const stroke = isConfirmed ? "var(--rs-ok)"
    : isPending ? "var(--rs-ok)"
    : isCancelled ? "var(--rs-bad-line)"
    : "var(--rs-off-line)";
  const textCol = (isConfirmed && confirmedFilled) ? "white"
    : isConfirmed ? "var(--rs-ok)"
    : isPending ? "var(--rs-ok)"
    : isCancelled ? "var(--rs-bad)"
    : "var(--rs-off)";

  return (
    <g onClick={onClick} style={{ cursor: "pointer" }}>
      <rect x={x + 1} y={y} width={Math.max(2, w - 2)} height={h} rx="3"
        fill={fill} stroke={stroke} strokeWidth={isPending ? "1.5" : "1"}
        strokeDasharray={isPending ? "5 3" : ""}
        opacity={isCompleted ? 0.65 : 1} />
      {isSel && (
        <rect x={x - 1} y={y - 2} width={w + 2} height={h + 4} rx="4"
          fill="none" stroke="var(--rs-accent)" strokeWidth="1.5" />
      )}
      {w > 50 && (
        <>
          <text x={x + 8} y={y + 16}
            fontFamily="IBM Plex Sans, system-ui, sans-serif"
            fontSize="11" fontWeight="500" fill={textCol}
            textDecoration={isCancelled ? "line-through" : "none"}
            style={{ pointerEvents: "none" }}>
            {tg?.name}
          </text>
          {h > 26 && w > 80 && (
            <text x={x + 8} y={y + 30}
              fontFamily="IBM Plex Mono, ui-monospace, monospace"
              fontSize="10" fill={(isConfirmed && confirmedFilled) ? "rgba(255,255,255,0.85)" : "var(--rs-ink-3)"}
              style={{ pointerEvents: "none" }}>
              {window.fmtHourMono(r.startHour)}–{window.fmtHourMono(r.endHour)}
            </text>
          )}
        </>
      )}
      {isCancelled && w > 8 && (
        <line x1={x + 4} y1={y + h / 2} x2={x + w - 4} y2={y + h / 2}
          stroke="var(--rs-bad)" strokeWidth="1" />
      )}
    </g>
  );
}

Object.assign(window, { ScheduleTimeline });
