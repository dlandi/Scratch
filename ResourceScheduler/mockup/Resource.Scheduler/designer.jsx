/* global React */
const { useState: useStateD, useMemo: useMemoD, useRef: useRefD, useEffect: useEffectD, useCallback: useCallbackD } = React;

// ============================================================
// DeviceGroupDesigner — the marquee screen.
// SVG canvas with member nodes + connection edges.
// Left: filterable device picker chips. Right: properties panel.
// ============================================================

function DeviceGroupDesigner({ initialGroupId = "g1", onOpenSchedule, tweaks = {} }) {
  // Working copy of groups so the user can mutate locally
  const [groups, setGroups] = useStateD(() => JSON.parse(JSON.stringify(window.DeviceGroups)));
  const [activeId, setActiveId] = useStateD(initialGroupId);
  const group = groups.find(g => g.id === activeId);

  const [pickerFilter, setPickerFilter] = useStateD("");
  const [pickerStatus, setPickerStatus] = useStateD("All");
  const [selectedDevice, setSelectedDevice] = useStateD(null);   // selected node id
  const [pendingEdge, setPendingEdge] = useStateD(null);          // {from} when connecting
  const [zoom, setZoom] = useStateD(1);

  const devicesById = useMemoD(() => Object.fromEntries(window.Devices.map(d => [d.id, d])), []);

  // Active membership across other groups (for "locked" state)
  const memberOfActive = useMemoD(() => {
    const m = {};
    groups.forEach(g => {
      if (g.status === "Active" && g.id !== activeId) {
        g.deviceIds.forEach(id => { m[id] = g; });
      }
    });
    return m;
  }, [groups, activeId]);

  // Validation messages per spec rules
  const validation = useMemoD(() => {
    const msgs = [];
    if (!group) return msgs;
    if (group.deviceIds.length === 0) {
      msgs.push({ rule: "R7", level: "warn", text: "Group has zero members. Cannot be activated." });
    }
    const blockingActive = group.deviceIds.filter(id => memberOfActive[id]);
    if (blockingActive.length) {
      msgs.push({
        rule: "R3", level: "bad",
        text: `${blockingActive.length} device(s) already in another Active group: ${blockingActive.map(id => devicesById[id].name).join(", ")}.`,
      });
    }
    const badStatus = group.deviceIds.filter(id => {
      const s = devicesById[id]?.status;
      return s === "Offline" || s === "Maintenance" || s === "Retired";
    });
    if (badStatus.length) {
      msgs.push({
        rule: "R5", level: "bad",
        text: `${badStatus.length} device(s) cannot be in an Active group: ${badStatus.map(id => devicesById[id].name + " (" + devicesById[id].status + ")").join(", ")}.`,
      });
    }
    if (msgs.length === 0) {
      msgs.push({ rule: "OK", level: "info", text: "Topology valid. Ready to activate." });
    }
    return msgs;
  }, [group, memberOfActive, devicesById]);

  // ----- Mutations -----
  const updateGroup = (patch) => {
    setGroups(prev => prev.map(g => g.id === activeId ? { ...g, ...patch } : g));
  };

  const toggleMember = (deviceId) => {
    if (!group) return;
    if (memberOfActive[deviceId]) return; // R3
    const inGroup = group.deviceIds.includes(deviceId);
    if (inGroup) {
      // remove + drop edges
      updateGroup({
        deviceIds: group.deviceIds.filter(x => x !== deviceId),
        connections: group.connections.filter(c => c.from !== deviceId && c.to !== deviceId),
        layout: Object.fromEntries(Object.entries(group.layout).filter(([k]) => k !== deviceId)),
      });
      if (selectedDevice === deviceId) setSelectedDevice(null);
    } else {
      // add — auto position in a free spot
      const used = Object.values(group.layout || {});
      const candidate = pickFreePosition(used);
      updateGroup({
        deviceIds: [...group.deviceIds, deviceId],
        layout: { ...group.layout, [deviceId]: candidate },
      });
    }
  };

  const removeEdge = (cid) => {
    updateGroup({ connections: group.connections.filter(c => c.id !== cid) });
  };

  const addEdge = (from, to) => {
    if (from === to) return;
    if (group.connections.some(c =>
      (c.from === from && c.to === to) || (c.from === to && c.to === from)
    )) return;
    updateGroup({
      connections: [...group.connections, {
        id: "c" + Math.random().toString(36).slice(2, 8),
        from, to, label: "—",
      }],
    });
  };

  // ----- Picker filter -----
  const filteredDevices = useMemoD(() => {
    return window.Devices.filter(d => {
      if (pickerStatus !== "All" && d.status !== pickerStatus) return false;
      if (pickerFilter && !d.name.toLowerCase().includes(pickerFilter.toLowerCase())) return false;
      return true;
    });
  }, [pickerFilter, pickerStatus]);

  if (!group) return null;

  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">05 · Device-Group Designer</div>
          <h1 className="rs-screen-title">{group.name}</h1>
          <p className="rs-screen-sub rs-mono-sm">
            {group.deviceIds.length} devices · {group.connections.length} connections · id <span style={{ color: "var(--rs-ink-2)" }}>{group.id}</span>
          </p>
        </div>
        <div className="rs-screen-actions">
          <select className="rs-select" style={{ width: 240 }}
            value={activeId} onChange={(e) => setActiveId(e.target.value)}>
            {groups.map(g => (
              <option key={g.id} value={g.id}>{g.name} · {g.status}</option>
            ))}
          </select>
          <button className="rs-btn rs-btn--ghost">Discard</button>
          <button className="rs-btn rs-btn--primary">Save</button>
        </div>
      </div>

      <div className="rs-designer">
        {/* LEFT: device picker */}
        <div className="rs-pane">
          <div className="rs-pane-head">
            <div className="rs-pane-title">
              <span>Device Picker</span>
              <span className="rs-mono-sm">{filteredDevices.length}</span>
            </div>
            <div className="rs-search" style={{ marginBottom: 8 }}>
              <svg width="14" height="14" viewBox="0 0 24 24" fill="none">
                <circle cx="11" cy="11" r="6" stroke="currentColor" strokeWidth="1.5" />
                <path d="m20 20-4-4" stroke="currentColor" strokeWidth="1.5" />
              </svg>
              <input className="rs-input" placeholder="Search devices…"
                value={pickerFilter} onChange={(e) => setPickerFilter(e.target.value)} />
            </div>
            <div className="rs-filter-group">
              {["All", "Available", "Maintenance", "Offline", "Retired"].map(s => (
                <button key={s} className={pickerStatus === s ? "is-active" : ""}
                  onClick={() => setPickerStatus(s)}>{s.toUpperCase()}</button>
              ))}
            </div>
          </div>
          <div className="rs-pane-body rs-pane-body--tight">
            <div className="rs-mono-sm" style={{ padding: "0 4px 6px" }}>
              Click a chip to add or remove from the group. Locked chips are already in another active group.
            </div>
            <div className="rs-chips">
              {filteredDevices.map(d => {
                const inGroup = group.deviceIds.includes(d.id);
                const locked = !!memberOfActive[d.id] && !inGroup;
                return (
                  <DeviceChip key={d.id}
                    device={d}
                    inGroup={inGroup}
                    locked={locked}
                    lockedBy={memberOfActive[d.id]?.name}
                    onClick={() => !locked && toggleMember(d.id)}
                  />
                );
              })}
            </div>
          </div>
        </div>

        {/* CENTER: canvas */}
        <DesignerCanvas
          group={group}
          devicesById={devicesById}
          memberOfActive={memberOfActive}
          selectedDevice={selectedDevice}
          setSelectedDevice={setSelectedDevice}
          pendingEdge={pendingEdge}
          setPendingEdge={setPendingEdge}
          updateLayout={(id, p) => updateGroup({ layout: { ...group.layout, [id]: p } })}
          addEdge={addEdge}
          removeEdge={removeEdge}
          zoom={zoom}
          setZoom={setZoom}
          nodeShape={tweaks.nodeShape || "module"}
        />

        {/* RIGHT: properties */}
        <div className="rs-pane">
          <div className="rs-pane-head">
            <div className="rs-pane-title">
              <span>Properties</span>
            </div>
          </div>
          <div style={{ overflow: "auto", flex: 1 }}>
            <div className="rs-prop-section">
              <div className="rs-field">
                <label className="rs-label">Group name</label>
                <input className="rs-input" value={group.name}
                  onChange={(e) => updateGroup({ name: e.target.value })} />
              </div>
              <div className="rs-field">
                <label className="rs-label">Status</label>
                <div className="rs-toggle">
                  <button className={group.status === "Active" ? "is-active" : ""}
                    onClick={() => updateGroup({ status: "Active" })}>ACTIVE</button>
                  <button className={`rs-toggle--off ${group.status === "Inactive" ? "is-active" : ""}`}
                    onClick={() => updateGroup({ status: "Inactive" })}>INACTIVE</button>
                </div>
              </div>
            </div>

            <div className="rs-prop-section">
              <div className="rs-pane-title" style={{ marginBottom: 10 }}>
                <span>Validation</span>
                <span className="rs-mono-sm">R3 · R5 · R6 · R7</span>
              </div>
              <div className="rs-stack-2">
                {validation.map((m, i) => (
                  <div key={i}
                    className={`rs-banner ${m.level === "warn" ? "rs-banner--warn" : m.level === "info" ? "rs-banner--info" : ""}`}>
                    <div style={{ flex: 1 }}>{m.text}</div>
                    <span className="rs-banner-rule">{m.rule}</span>
                  </div>
                ))}
              </div>
            </div>

            {selectedDevice && (
              <div className="rs-prop-section">
                <div className="rs-pane-title" style={{ marginBottom: 10 }}>
                  <span>Selected node</span>
                  <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => setSelectedDevice(null)}>Deselect</button>
                </div>
                <SelectedDevicePanel
                  device={devicesById[selectedDevice]}
                  group={group}
                  startConnect={() => setPendingEdge({ from: selectedDevice })}
                  pending={pendingEdge}
                  onRemove={() => toggleMember(selectedDevice)}
                />
              </div>
            )}

            <div className="rs-prop-section">
              <div className="rs-pane-title" style={{ marginBottom: 10 }}>
                <span>Connections</span>
                <span className="rs-mono-sm">{group.connections.length}</span>
              </div>
              <div className="rs-stack-2">
                {group.connections.length === 0 && (
                  <div className="rs-mono-sm">No connections yet. Select a node and click "Add cable".</div>
                )}
                {group.connections.map(c => {
                  const a = devicesById[c.from], b = devicesById[c.to];
                  return (
                    <div key={c.id} className="rs-prop-row" style={{ alignItems: "center" }}>
                      <div style={{ flex: 1, fontSize: 12 }}>
                        <span className="rs-mono">{a?.name}</span>
                        <span style={{ color: "var(--rs-ink-4)", margin: "0 6px" }}>—</span>
                        <span className="rs-mono">{b?.name}</span>
                      </div>
                      <input className="rs-input" style={{ width: 80, height: 24, fontFamily: "var(--rs-font-mono)", fontSize: 11 }}
                        value={c.label}
                        onChange={(e) => updateGroup({
                          connections: group.connections.map(x => x.id === c.id ? { ...x, label: e.target.value } : x),
                        })} />
                      <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => removeEdge(c.id)}>×</button>
                    </div>
                  );
                })}
              </div>
            </div>

            <div className="rs-prop-section">
              <div className="rs-pane-title" style={{ marginBottom: 10 }}>
                <span>Upcoming on this group</span>
              </div>
              <UpcomingForGroup groupId={group.id} onOpenSchedule={onOpenSchedule} />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

function DeviceChip({ device, inGroup, locked, lockedBy, onClick }) {
  const cls = window.deviceStatusToClass(device.status);
  return (
    <div className="rs-chip" data-locked={locked} data-in-group={inGroup} onClick={onClick}
      title={locked ? `Locked: in active group "${lockedBy}"` : ""}>
      <svg width="16" height="16" viewBox="0 0 16 16">
        <rect x="2" y="3" width="12" height="10" rx="1.5"
          fill={`var(--rs-${cls}-soft)`}
          stroke={`var(--rs-${cls})`} strokeWidth="1"
          strokeDasharray={inGroup ? "" : (locked ? "" : "2 2")} />
        <circle cx="13" cy="3" r="1.5" fill={`var(--rs-${cls})`} />
      </svg>
      <span className="rs-chip-name rs-mono">{device.name}</span>
      {locked && (
        <svg width="11" height="11" viewBox="0 0 16 16" style={{ color: "var(--rs-ink-4)" }}>
          <rect x="3" y="7" width="10" height="7" rx="1" fill="none" stroke="currentColor" strokeWidth="1.2" />
          <path d="M5 7V5a3 3 0 016 0v2" fill="none" stroke="currentColor" strokeWidth="1.2" />
        </svg>
      )}
      <span className="rs-chip-meta">{inGroup ? "·in" : (locked ? "·lock" : "·free")}</span>
    </div>
  );
}

function pickFreePosition(used) {
  // Try a small grid of candidates and return the one with maximum min-distance from used
  const candidates = [];
  for (let x = 0.2; x <= 0.85; x += 0.15) {
    for (let y = 0.25; y <= 0.8; y += 0.15) {
      candidates.push([x, y]);
    }
  }
  let best = candidates[0], bestScore = -Infinity;
  for (const c of candidates) {
    let minD = Infinity;
    for (const u of used) {
      const dx = c[0] - u[0], dy = c[1] - u[1];
      minD = Math.min(minD, Math.sqrt(dx * dx + dy * dy));
    }
    if (minD > bestScore) { bestScore = minD; best = c; }
  }
  return best;
}

// ===== Canvas =====

function DesignerCanvas({
  group, devicesById, memberOfActive,
  selectedDevice, setSelectedDevice,
  pendingEdge, setPendingEdge,
  updateLayout, addEdge, removeEdge, zoom, setZoom,
  nodeShape = "module",
}) {
  const SHAPES = {
    module: { w: 100, h: 40, rx: 2,  showStatusText: true,  showCaps: true,  labelDy: -2 },
    chip:   { w: 104, h: 30, rx: 15, showStatusText: false, showCaps: false, labelDy: 4 },
    tag:    { w: 84,  h: 26, rx: 3,  showStatusText: false, showCaps: false, labelDy: 4 },
  };
  const shape = SHAPES[nodeShape] || SHAPES.module;
  const wrapRef = useRefD(null);
  const svgRef = useRefD(null);
  const [W, setW] = useStateD(900);
  const [H, setH] = useStateD(560);
  const [drag, setDrag] = useStateD(null);  // {id, dx, dy}
  const [hover, setHover] = useStateD(null);
  const [mouse, setMouse] = useStateD({ x: 0, y: 0 });

  useEffectD(() => {
    if (!wrapRef.current) return;
    const ro = new ResizeObserver(([entry]) => {
      const r = entry.contentRect;
      setW(Math.max(400, r.width));
      setH(Math.max(300, r.height));
    });
    ro.observe(wrapRef.current);
    return () => ro.disconnect();
  }, []);

  const toSvg = (e) => {
    const r = svgRef.current.getBoundingClientRect();
    return { x: (e.clientX - r.left) / r.width, y: (e.clientY - r.top) / r.height };
  };

  const onMouseMove = (e) => {
    const p = toSvg(e);
    setMouse(p);
    if (drag) {
      const nx = Math.max(0.04, Math.min(0.96, p.x - drag.dx));
      const ny = Math.max(0.06, Math.min(0.92, p.y - drag.dy));
      updateLayout(drag.id, [nx, ny]);
    }
  };
  const onMouseUp = () => setDrag(null);

  const PADX = 40, PADY = 30;
  const sx = (nx) => PADX + nx * (W - 2 * PADX);
  const sy = (ny) => PADY + ny * (H - 2 * PADY);

  return (
    <div className="rs-canvas-wrap" ref={wrapRef}>
      <div className="rs-canvas-toolbar">
        <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => setZoom(Math.min(1.6, zoom + 0.1))}>＋</button>
        <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => setZoom(Math.max(0.6, zoom - 0.1))}>－</button>
        <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => setZoom(1)}>fit</button>
        <span style={{ width: 1, background: "var(--rs-line)", margin: "0 2px" }} />
        <button className={`rs-btn rs-btn--ghost rs-btn--sm ${pendingEdge ? "" : ""}`}
          onClick={() => {
            if (pendingEdge) setPendingEdge(null);
            else if (selectedDevice) setPendingEdge({ from: selectedDevice });
          }}
          disabled={!selectedDevice && !pendingEdge}>
          {pendingEdge ? "cancel cable" : "add cable"}
        </button>
      </div>

      <div className="rs-canvas">
        <svg ref={svgRef}
          viewBox={`0 0 ${W} ${H}`}
          onMouseMove={onMouseMove}
          onMouseUp={onMouseUp}
          onClick={() => setSelectedDevice(null)}>
          <g style={{ transform: `scale(${zoom})`, transformOrigin: `${W/2}px ${H/2}px` }}>
            {/* edges */}
            {group.connections.map(c => {
              const a = group.layout[c.from], b = group.layout[c.to];
              if (!a || !b) return null;
              const ax = sx(a[0]), ay = sy(a[1]);
              const bx = sx(b[0]), by = sy(b[1]);
              const mx = (ax + bx) / 2, my = (ay + by) / 2;
              const dx = bx - ax, dy = by - ay;
              const len = Math.sqrt(dx * dx + dy * dy);
              const nx = -dy / len, ny = dx / len;
              return (
                <g key={c.id}>
                  <line x1={ax} y1={ay} x2={bx} y2={by}
                    stroke="var(--rs-ink-3)" strokeWidth="1.5"
                    strokeLinecap="round" />
                  {c.label && c.label !== "—" && (
                    <g transform={`translate(${mx + nx * 10}, ${my + ny * 10})`}>
                      <rect x="-22" y="-9" width="44" height="14" rx="2"
                        fill="var(--rs-surface-2)" stroke="var(--rs-line)" strokeWidth="0.75" />
                      <text x="0" y="0" dominantBaseline="middle" textAnchor="middle"
                        fontFamily="IBM Plex Mono, ui-monospace, monospace"
                        fontSize="9" fill="var(--rs-ink-3)" letterSpacing="0.04em">
                        {c.label}
                      </text>
                    </g>
                  )}
                </g>
              );
            })}

            {/* pending edge */}
            {pendingEdge && (() => {
              const a = group.layout[pendingEdge.from];
              if (!a) return null;
              return (
                <line x1={sx(a[0])} y1={sy(a[1])} x2={sx(mouse.x)} y2={sy(mouse.y)}
                  stroke="var(--rs-accent)" strokeWidth="1.5" strokeDasharray="4 3" />
              );
            })()}

            {/* nodes */}
            {group.deviceIds.map(id => {
              const d = devicesById[id];
              const p = group.layout[id];
              if (!d || !p) return null;
              const cx = sx(p[0]), cy = sy(p[1]);
              const cls = window.deviceStatusToClass(d.status);
              const isSelected = selectedDevice === id;
              const isHover = hover === id;
              return (
                <g key={id}
                  style={{ cursor: drag?.id === id ? "grabbing" : "grab" }}
                  onMouseDown={(e) => {
                    e.stopPropagation();
                    const m = toSvg(e);
                    setDrag({ id, dx: m.x - p[0], dy: m.y - p[1] });
                    setSelectedDevice(id);
                    if (pendingEdge && pendingEdge.from !== id) {
                      addEdge(pendingEdge.from, id);
                      setPendingEdge(null);
                    }
                  }}
                  onMouseEnter={() => setHover(id)}
                  onMouseLeave={() => setHover(null)}
                  onClick={(e) => e.stopPropagation()}>
                  {/* selection halo */}
                  {(isSelected || isHover) && (
                    <rect x={cx - shape.w/2 - 6} y={cy - shape.h/2 - 6} width={shape.w + 12} height={shape.h + 12} rx={shape.rx + 2}
                      fill="none" stroke="var(--rs-accent)" strokeWidth={isSelected ? 1.5 : 1}
                      strokeDasharray={isSelected ? "" : "3 3"} opacity="0.7" />
                  )}
                  {/* node body */}
                  <rect x={cx - shape.w/2} y={cy - shape.h/2} width={shape.w} height={shape.h} rx={shape.rx}
                    fill={`var(--rs-${cls}-soft)`}
                    stroke={`var(--rs-${cls})`}
                    strokeWidth="1.5" />
                  {/* status dot top-right */}
                  <circle cx={cx + shape.w/2 - 6} cy={cy - shape.h/2 + 6} r="3" fill={`var(--rs-${cls})`} />
                  {/* connector caps */}
                  {shape.showCaps && (
                    <>
                      <circle cx={cx - shape.w/2} cy={cy} r="2" fill="var(--rs-ink-3)" />
                      <circle cx={cx + shape.w/2} cy={cy} r="2" fill="var(--rs-ink-3)" />
                    </>
                  )}
                  {/* label */}
                  <text x={cx} y={cy + shape.labelDy} textAnchor="middle"
                    fontFamily="var(--rs-font-mono)"
                    fontSize="11" fontWeight="500" fill="var(--rs-ink)">
                    {d.name}
                  </text>
                  {shape.showStatusText && (
                    <text x={cx} y={cy + 11} textAnchor="middle"
                      fontFamily="var(--rs-font-sans)"
                      fontSize="9" fill="var(--rs-ink-3)">
                      {d.status}
                    </text>
                  )}
                </g>
              );
            })}

            {/* "free to add" ghost nodes for non-members near edges, just hint */}
          </g>
        </svg>
      </div>

      <div className="rs-canvas-legend">
        <span><span className="swatch" style={{ background: "var(--rs-ok-soft)", border: "1.5px solid var(--rs-ok)", borderRadius: 2 }} />Available</span>
        <span><span className="swatch" style={{ background: "var(--rs-warn-soft)", border: "1.5px solid var(--rs-warn)", borderRadius: 2 }} />Maintenance</span>
        <span><span className="swatch" style={{ background: "var(--rs-off-soft)", border: "1.5px solid var(--rs-off)", borderRadius: 2 }} />Offline</span>
        <span style={{ borderLeft: "1px solid var(--rs-line)", paddingLeft: 12 }}>
          <span className="swatch" style={{ background: "transparent", border: "1.5px solid var(--rs-ink)", borderRadius: 2 }} /> in group
        </span>
        <span>
          <span className="swatch" style={{ background: "transparent", border: "1.5px dashed var(--rs-ink)", borderRadius: 2 }} /> free to add
        </span>
      </div>

      <div className="rs-canvas-coords">
        x={mouse.x.toFixed(2)} · y={mouse.y.toFixed(2)} · zoom={zoom.toFixed(2)}
      </div>
    </div>
  );
}

function SelectedDevicePanel({ device, group, startConnect, pending, onRemove }) {
  if (!device) return null;
  return (
    <div className="rs-stack-2">
      <div className="rs-prop-row"><span className="rs-kv">Device</span><b className="rs-mono">{device.name}</b></div>
      <div className="rs-prop-row"><span className="rs-kv">Status</span>
        <Pill status={device.status} kind="device" /></div>
      <div className="rs-prop-row"><span className="rs-kv">Building</span>
        <span className="rs-mono">{window.Buildings.find(b => b.id === device.buildingId)?.name}</span></div>
      <div className="rs-row" style={{ marginTop: 10, gap: 6 }}>
        <button className="rs-btn rs-btn--sm" onClick={startConnect} disabled={!!pending}>
          {pending ? "Click target node…" : "Add cable"}
        </button>
        <button className="rs-btn rs-btn--sm rs-btn--danger" onClick={onRemove}>Remove from group</button>
      </div>
    </div>
  );
}

function UpcomingForGroup({ groupId, onOpenSchedule }) {
  const items = window.Reservations
    .filter(r => r.groupId === groupId && r.dayOffset >= 0 && r.status !== "Cancelled" && r.status !== "Completed")
    .sort((a, b) => a.dayOffset - b.dayOffset || a.startHour - b.startHour)
    .slice(0, 4);
  if (!items.length) {
    return <div className="rs-mono-sm">No upcoming reservations.</div>;
  }
  return (
    <div className="rs-stack-2">
      {items.map(r => {
        const tg = window.TestGroups.find(t => t.id === r.testGroupId);
        const d = window.dateForOffset(r.dayOffset);
        return (
          <div key={r.id} className="rs-prop-row" style={{ alignItems: "center" }}>
            <div style={{ flex: 1 }}>
              <div className="rs-mono" style={{ fontSize: 11, color: "var(--rs-ink-3)" }}>
                {d.toLocaleDateString("en", { weekday: "short" }).toUpperCase()} {String(d.getDate()).padStart(2, "0")}
                {" · "}
                {window.fmtHourMono(r.startHour)}–{window.fmtHourMono(r.endHour)}
              </div>
              <div style={{ fontSize: 12 }}>{tg?.name}</div>
            </div>
            <Pill status={r.status} kind="resv" />
          </div>
        );
      })}
      <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={onOpenSchedule}>Open schedule →</button>
    </div>
  );
}

Object.assign(window, { DeviceGroupDesigner });
