/* global React */
const { useState: useStateL, useMemo: useMemoL } = React;

// ============================================================
// Dashboard
// ============================================================

function Dashboard({ onNav }) {
  const today = window.Reservations.filter(r => r.dayOffset === 0);
  const activeNow = today.filter(r => r.status === "Confirmed" && r.startHour <= 11.25 && r.endHour > 11.25);
  const upcoming = window.Reservations
    .filter(r => (r.dayOffset > 0 || (r.dayOffset === 0 && r.startHour > 11.25)) && r.status !== "Cancelled" && r.status !== "Completed")
    .sort((a, b) => a.dayOffset - b.dayOffset || a.startHour - b.startHour)
    .slice(0, 6);
  const attention = window.Devices.filter(d => d.status === "Maintenance" || d.status === "Offline");

  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">01 · Dashboard</div>
          <h1 className="rs-screen-title">Today, Friday May 8 · 11:15</h1>
          <p className="rs-screen-sub rs-mono-sm">
            {window.DeviceGroups.filter(g => g.status === "Active").length} active groups · {window.Devices.length} devices · {window.TestGroups.length} test-groups
          </p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn" onClick={() => onNav("schedule")}>Open schedule</button>
          <button className="rs-btn rs-btn--primary" onClick={() => onNav("reservation")}>+ New reservation</button>
        </div>
      </div>

      <div className="rs-dash">
        <div className="rs-dash-grid">
          <div className="rs-dash-card">
            <div className="rs-dash-card-head">
              <div>
                <div className="rs-dash-card-eyebrow">Active groups today</div>
                <div className="rs-dash-card-num">{activeNow.length}</div>
              </div>
              <Pill status="Active" kind="group">RUNNING</Pill>
            </div>
            <div className="rs-dash-card-foot">
              {activeNow.length === 0 && <div className="rs-mono-sm">No groups active right now.</div>}
              {activeNow.map(r => {
                const g = window.DeviceGroups.find(x => x.id === r.groupId);
                const t = window.TestGroups.find(x => x.id === r.testGroupId);
                return (
                  <div key={r.id} className="rs-dash-row">
                    <span className="rs-dash-row-name">{g?.name}</span>
                    <span className="rs-dash-row-meta">{t?.name} · until {window.fmtHourMono(r.endHour)}</span>
                  </div>
                );
              })}
            </div>
          </div>

          <div className="rs-dash-card">
            <div className="rs-dash-card-head">
              <div>
                <div className="rs-dash-card-eyebrow">Upcoming reservations</div>
                <div className="rs-dash-card-num">{upcoming.length}</div>
              </div>
              <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => onNav("schedule")}>view →</button>
            </div>
            <div className="rs-dash-card-foot">
              {upcoming.slice(0, 5).map(r => {
                const g = window.DeviceGroups.find(x => x.id === r.groupId);
                const d = window.dateForOffset(r.dayOffset);
                return (
                  <div key={r.id} className="rs-dash-row">
                    <span className="rs-dash-row-name">
                      <span className={`rs-dot rs-dot--${window.resvStatusToClass(r.status)}`} style={{ width: 6, height: 6 }} />
                      {g?.name}
                    </span>
                    <span className="rs-dash-row-meta">
                      {d.toLocaleDateString("en", { weekday: "short" }).toUpperCase()} {window.fmtHourMono(r.startHour)}
                    </span>
                  </div>
                );
              })}
            </div>
          </div>

          <div className="rs-dash-card">
            <div className="rs-dash-card-head">
              <div>
                <div className="rs-dash-card-eyebrow">Devices needing attention</div>
                <div className="rs-dash-card-num">{attention.length}</div>
              </div>
              <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => onNav("devices")}>view →</button>
            </div>
            <div className="rs-dash-card-foot">
              {attention.map(d => (
                <div key={d.id} className="rs-dash-row">
                  <span className="rs-dash-row-name">
                    <StatusDot status={d.status} /><span className="rs-mono">{d.name}</span>
                  </span>
                  <span className="rs-dash-row-meta">{d.status} · {window.Buildings.find(b => b.id === d.buildingId)?.name}</span>
                </div>
              ))}
            </div>
          </div>
        </div>

        {/* Mini schedule preview */}
        <div className="rs-card">
          <div className="rs-card-head">
            <span className="rs-card-title">Today's groups · 06:00 → 22:00</span>
            <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => onNav("schedule")}>open timeline →</button>
          </div>
          <div style={{ padding: "8px 16px 16px" }}>
            <DashboardMiniTimeline />
          </div>
        </div>
      </div>
    </div>
  );
}

function DashboardMiniTimeline() {
  const groups = window.DeviceGroups.filter(g => g.status === "Active");
  const W = 1100, ROW_H = 28;
  const PAD_L = 160;
  const START_H = 6, END_H = 22;
  const HOURS = END_H - START_H;
  const innerW = W - PAD_L;
  const xH = (h) => PAD_L + ((h - START_H) / HOURS) * innerW;
  const items = window.Reservations.filter(r => r.dayOffset === 0);
  const totalH = 24 + groups.length * ROW_H;
  return (
    <svg viewBox={`0 0 ${W} ${totalH}`} width="100%" height={totalH} style={{ display: "block" }}>
      {/* hour ticks */}
      {Array.from({ length: HOURS + 1 }, (_, i) => {
        const h = START_H + i;
        const x = xH(h);
        return (
          <g key={i}>
            <line x1={x} y1="20" x2={x} y2={totalH} stroke="var(--rs-line)" strokeDasharray={i % 4 === 0 ? "" : "2 4"} />
            {i % 4 === 0 && (
              <text x={x + 3} y="14" fontFamily="IBM Plex Mono, ui-monospace, monospace"
                fontSize="9" fill="var(--rs-ink-4)">{String(h).padStart(2, "0")}:00</text>
            )}
          </g>
        );
      })}
      {/* now */}
      <line x1={xH(11.25)} y1="20" x2={xH(11.25)} y2={totalH} stroke="var(--rs-bad)" strokeWidth="1" strokeDasharray="3 3" />
      {/* rows */}
      {groups.map((g, i) => {
        const y = 24 + i * ROW_H;
        return (
          <g key={g.id}>
            <text x="8" y={y + 18} fontSize="11" fill="var(--rs-ink-2)">{g.name}</text>
            <line x1={PAD_L} y1={y + ROW_H} x2={W} y2={y + ROW_H} stroke="var(--rs-line)" />
            {items.filter(r => r.groupId === g.id).map(r => {
              const x1 = xH(r.startHour), x2 = xH(r.endHour);
              const cls = window.resvStatusToClass(r.status);
              const fill = r.status === "Confirmed" ? "var(--rs-ok)"
                : r.status === "Pending" ? "transparent"
                : r.status === "Cancelled" ? "var(--rs-bad-soft)"
                : "var(--rs-off-soft)";
              const stroke = r.status === "Pending" ? "var(--rs-ok)" : `var(--rs-${cls})`;
              return (
                <rect key={r.id} x={x1} y={y + 4} width={Math.max(2, x2 - x1)} height={ROW_H - 10} rx="2"
                  fill={fill} stroke={stroke} strokeWidth="1"
                  strokeDasharray={r.status === "Pending" ? "4 3" : ""}
                  opacity={r.status === "Completed" ? 0.6 : 1} />
              );
            })}
          </g>
        );
      })}
    </svg>
  );
}

// ============================================================
// Devices list
// ============================================================

function DevicesList() {
  const [q, setQ] = useStateL("");
  const [status, setStatus] = useStateL("All");
  const [building, setBuilding] = useStateL("All");
  const filtered = useMemoL(() => window.Devices.filter(d => {
    if (status !== "All" && d.status !== status) return false;
    if (building !== "All" && d.buildingId !== building) return false;
    if (q && !d.name.toLowerCase().includes(q.toLowerCase())) return false;
    return true;
  }), [q, status, building]);
  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">03 · Devices</div>
          <h1 className="rs-screen-title">Devices</h1>
          <p className="rs-screen-sub rs-mono-sm">{window.Devices.length} total · {window.Devices.filter(d => d.status === "Available").length} available · {window.Devices.filter(d => d.status === "Maintenance" || d.status === "Offline").length} need attention</p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn">Bulk edit</button>
          <button className="rs-btn rs-btn--primary">+ New device</button>
        </div>
      </div>
      <div className="rs-filterbar">
        <div className="rs-search">
          <svg width="14" height="14" viewBox="0 0 24 24" fill="none">
            <circle cx="11" cy="11" r="6" stroke="currentColor" strokeWidth="1.5" />
            <path d="m20 20-4-4" stroke="currentColor" strokeWidth="1.5" />
          </svg>
          <input className="rs-input" value={q} onChange={(e) => setQ(e.target.value)} placeholder="Search by name…" />
        </div>
        <div className="rs-filter-group">
          {["All", "Available", "Maintenance", "Offline", "Retired"].map(s => (
            <button key={s} className={status === s ? "is-active" : ""} onClick={() => setStatus(s)}>{s.toUpperCase()}</button>
          ))}
        </div>
        <select className="rs-select" style={{ width: 200 }} value={building} onChange={(e) => setBuilding(e.target.value)}>
          <option value="All">All buildings</option>
          {window.Buildings.map(b => <option key={b.id} value={b.id}>{b.name}</option>)}
        </select>
        <div style={{ flex: 1 }} />
        <span className="rs-mono-sm">{filtered.length} shown</span>
      </div>
      <div className="rs-list-wrap">
        <div className="rs-table-wrap">
          <table className="rs-table">
            <thead>
              <tr>
                <th style={{ width: 40 }}>#</th>
                <th>Name</th>
                <th>Status</th>
                <th>Building</th>
                <th>Device-Group</th>
                <th style={{ width: 120 }}></th>
              </tr>
            </thead>
            <tbody>
              {filtered.map((d, i) => {
                const g = window.DeviceGroups.find(x => x.id === d.groupId);
                const b = window.Buildings.find(x => x.id === d.buildingId);
                return (
                  <tr key={d.id}>
                    <td className="num">{String(i + 1).padStart(2, "0")}</td>
                    <td><span className="rs-mono">{d.name}</span></td>
                    <td><Pill status={d.status} kind="device" /></td>
                    <td>{b?.name}</td>
                    <td>{g ? <span>{g.name} <Pill status={g.status} kind="group" /></span> : <span className="rs-muted">Unassigned</span>}</td>
                    <td><button className="rs-btn rs-btn--ghost rs-btn--sm">Edit</button></td>
                  </tr>
                );
              })}
              {filtered.length === 0 && (
                <tr><td colSpan="6" style={{ padding: 32, textAlign: "center", color: "var(--rs-ink-3)" }}>No devices match the current filter.</td></tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

// ============================================================
// DeviceGroups list (cards)
// ============================================================

function DeviceGroupsList({ onOpenDesigner }) {
  const [filter, setFilter] = useStateL("All");
  const groups = window.DeviceGroups.filter(g => filter === "All" || g.status === filter);
  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">04 · Device-Groups</div>
          <h1 className="rs-screen-title">Device-Groups</h1>
          <p className="rs-screen-sub rs-mono-sm">{window.DeviceGroups.filter(g => g.status === "Active").length} active · {window.DeviceGroups.filter(g => g.status !== "Active").length} draft</p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn rs-btn--primary" onClick={() => onOpenDesigner()}>+ New group</button>
        </div>
      </div>
      <div className="rs-filterbar">
        <div className="rs-filter-group">
          {["All", "Active", "Inactive"].map(s => (
            <button key={s} className={filter === s ? "is-active" : ""} onClick={() => setFilter(s)}>{s.toUpperCase()}</button>
          ))}
        </div>
        <div style={{ flex: 1 }} />
        <span className="rs-mono-sm">{groups.length} shown</span>
      </div>
      <div className="rs-list-wrap">
        <div className="rs-grid">
          {groups.map(g => {
            const next = window.Reservations.find(r => r.groupId === g.id && r.dayOffset >= 0 && r.status !== "Cancelled" && r.status !== "Completed");
            return (
              <div key={g.id} className="rs-group-card" onClick={() => onOpenDesigner(g.id)}>
                <div className="rs-group-card-head">
                  <h3 className="rs-group-card-name">{g.name}</h3>
                  <Pill status={g.status} kind="group" />
                </div>
                <div className="rs-group-card-thumb">
                  <MiniTopology group={g} />
                </div>
                <div className="rs-group-card-foot">
                  <span>{g.deviceIds.length} devices · {g.connections.length} cables</span>
                  <span>
                    {next
                      ? <>next: {window.dateForOffset(next.dayOffset).toLocaleDateString("en", { weekday: "short" })} {window.fmtHourMono(next.startHour)}</>
                      : <span className="rs-muted">no upcoming</span>}
                  </span>
                </div>
              </div>
            );
          })}
        </div>
      </div>
    </div>
  );
}

// ============================================================
// Buildings list + detail
// ============================================================

function BuildingsList() {
  const [openId, setOpenId] = useStateL(null);
  if (openId) {
    const b = window.Buildings.find(x => x.id === openId);
    const devices = window.Devices.filter(d => d.buildingId === openId);
    return (
      <div className="rs-screen">
        <div className="rs-screen-head">
          <div>
            <div className="rs-screen-eyebrow">02 · Buildings · Detail</div>
            <h1 className="rs-screen-title">{b.name}</h1>
            <p className="rs-screen-sub rs-mono-sm">{b.deviceCount} devices on site</p>
          </div>
          <div className="rs-screen-actions">
            <button className="rs-btn" onClick={() => setOpenId(null)}>← All buildings</button>
            <button className="rs-btn">Edit building</button>
          </div>
        </div>
        <div className="rs-detail">
          <div className="rs-detail-side">
            <div className="rs-label">Mailing address</div>
            <pre style={{
              fontFamily: "var(--rs-font-mono)", fontSize: 12, lineHeight: 1.6,
              background: "var(--rs-surface-2)", border: "1px solid var(--rs-line)",
              borderRadius: 2, padding: 12, margin: 0, whiteSpace: "pre-wrap",
            }}>{b.address}</pre>
            <div style={{ marginTop: 24 }}>
              <div className="rs-label">Composition</div>
              <BuildingMix devices={devices} />
            </div>
          </div>
          <div className="rs-detail-main">
            <div className="rs-table-wrap">
              <table className="rs-table">
                <thead>
                  <tr><th>Name</th><th>Status</th><th>Group</th><th></th></tr>
                </thead>
                <tbody>
                  {devices.map(d => {
                    const g = window.DeviceGroups.find(x => x.id === d.groupId);
                    return (
                      <tr key={d.id}>
                        <td><span className="rs-mono">{d.name}</span></td>
                        <td><Pill status={d.status} kind="device" /></td>
                        <td>{g ? g.name : <span className="rs-muted">Unassigned</span>}</td>
                        <td><button className="rs-btn rs-btn--ghost rs-btn--sm">Edit</button></td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    );
  }
  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">02 · Buildings</div>
          <h1 className="rs-screen-title">Buildings</h1>
          <p className="rs-screen-sub rs-mono-sm">{window.Buildings.length} sites · {window.Devices.length} devices</p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn rs-btn--primary">+ New building</button>
        </div>
      </div>
      <div className="rs-list-wrap">
        <div className="rs-table-wrap">
          <table className="rs-table">
            <thead>
              <tr><th style={{ width: 40 }}>#</th><th>Name</th><th>Address</th><th>Devices</th><th></th></tr>
            </thead>
            <tbody>
              {window.Buildings.map((b, i) => (
                <tr key={b.id} style={{ cursor: "pointer" }} onClick={() => setOpenId(b.id)}>
                  <td className="num">{String(i + 1).padStart(2, "0")}</td>
                  <td><span style={{ fontWeight: 500 }}>{b.name}</span></td>
                  <td><span className="rs-mono-sm" style={{ color: "var(--rs-ink-2)" }}>{b.address.split("\n")[0]}</span></td>
                  <td className="num">{b.deviceCount}</td>
                  <td><button className="rs-btn rs-btn--ghost rs-btn--sm">Open →</button></td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

function BuildingMix({ devices }) {
  const counts = { Available: 0, Maintenance: 0, Offline: 0, Retired: 0 };
  devices.forEach(d => counts[d.status]++);
  const total = devices.length || 1;
  const W = 280, H = 16;
  let x = 0;
  const segs = ["Available", "Maintenance", "Offline", "Retired"].map(s => {
    const w = (counts[s] / total) * W;
    const seg = { s, x, w };
    x += w;
    return seg;
  });
  return (
    <div>
      <svg width="100%" viewBox={`0 0 ${W} ${H}`} height={H} style={{ display: "block", border: "1px solid var(--rs-line)" }}>
        {segs.map(seg => (
          <rect key={seg.s} x={seg.x} y="0" width={seg.w} height={H}
            fill={`var(--rs-${window.deviceStatusToClass(seg.s)})`} />
        ))}
      </svg>
      <div style={{ display: "flex", flexWrap: "wrap", gap: 8, marginTop: 8 }}>
        {Object.entries(counts).map(([k, v]) => (
          <div key={k} className="rs-mono-sm" style={{ display: "flex", alignItems: "center", gap: 4 }}>
            <StatusDot status={k} />{k} <b style={{ color: "var(--rs-ink)" }}>{v}</b>
          </div>
        ))}
      </div>
    </div>
  );
}

// ============================================================
// Test-Groups — list + editor
// ============================================================

function TestGroupsList() {
  const [openId, setOpenId] = useStateL(window.TestGroups[0].id);
  const tg = window.TestGroups.find(t => t.id === openId);
  const peopleById = Object.fromEntries(window.People.map(p => [p.id, p]));
  const [members, setMembers] = useStateL(tg.memberIds);

  const updateMembers = (newIds) => setMembers(newIds);

  const removeMember = (pid) => updateMembers(members.filter(x => x !== pid));
  const addMember = (pid) => !members.includes(pid) && updateMembers([...members, pid]);

  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">06 · Test-Groups</div>
          <h1 className="rs-screen-title">Test-Groups</h1>
          <p className="rs-screen-sub rs-mono-sm">{window.TestGroups.length} teams · {window.People.length} people</p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn rs-btn--primary">+ New test-group</button>
        </div>
      </div>

      <div className="rs-detail" style={{ gridTemplateColumns: "320px 1fr" }}>
        <div className="rs-detail-side" style={{ padding: 0 }}>
          <div className="rs-pane-head" style={{ padding: "16px 20px" }}>
            <div className="rs-pane-title"><span>Teams</span><span className="rs-mono-sm">{window.TestGroups.length}</span></div>
          </div>
          <div className="rs-stack-2" style={{ padding: 8 }}>
            {window.TestGroups.map(t => {
              const isOpen = t.id === openId;
              const tPeople = t.memberIds.map(id => peopleById[id]).filter(Boolean);
              return (
                <div key={t.id}
                  onClick={() => { setOpenId(t.id); setMembers(t.memberIds); }}
                  style={{
                    padding: 12,
                    cursor: "pointer",
                    border: "1px solid",
                    borderColor: isOpen ? "var(--rs-ink)" : "var(--rs-line)",
                    background: isOpen ? "var(--rs-bg)" : "var(--rs-surface-2)",
                    borderRadius: 2,
                  }}>
                  <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 6 }}>
                    <span style={{ fontWeight: 500 }}>{t.name}</span>
                    <span className="rs-mono-sm">{t.memberIds.length} people</span>
                  </div>
                  <AvatarCluster people={tPeople} max={5} />
                </div>
              );
            })}
          </div>
        </div>

        <div className="rs-detail-main">
          <div style={{ display: "flex", justifyContent: "space-between", alignItems: "flex-end", marginBottom: 16 }}>
            <div>
              <div className="rs-label">Editing</div>
              <h2 style={{ margin: 0, fontSize: 20, fontWeight: 500 }}>{tg.name}</h2>
            </div>
            <div className="rs-row">
              <button className="rs-btn rs-btn--ghost">Rename</button>
              <button className="rs-btn rs-btn--danger">Delete team</button>
            </div>
          </div>

          <div className="rs-card" style={{ marginBottom: 16 }}>
            <div className="rs-card-head">
              <span className="rs-card-title">Members ({members.length})</span>
            </div>
            <div className="rs-card-body">
              {members.length === 0 ? (
                <div className="rs-empty">No members yet. Add people from the directory below.</div>
              ) : (
                <div style={{ display: "flex", flexDirection: "column", gap: 8 }}>
                  {members.map((pid, i) => {
                    const p = peopleById[pid];
                    return (
                      <div key={pid} style={{
                        display: "flex", alignItems: "center", gap: 12,
                        padding: 10, border: "1px solid var(--rs-line)", borderRadius: 2, background: "var(--rs-surface-2)",
                      }}>
                        <Avatar name={p.name} idx={i} size={28} />
                        <div style={{ flex: 1 }}>
                          <div style={{ fontWeight: 500 }}>{p.name}</div>
                          <div className="rs-mono-sm">{p.email}</div>
                        </div>
                        <button className="rs-btn rs-btn--ghost rs-btn--sm" onClick={() => removeMember(pid)}>Remove</button>
                      </div>
                    );
                  })}
                </div>
              )}
            </div>
          </div>

          <div className="rs-card">
            <div className="rs-card-head">
              <span className="rs-card-title">People directory</span>
              <span className="rs-mono-sm">click to add</span>
            </div>
            <div className="rs-card-body">
              <div style={{ display: "grid", gridTemplateColumns: "repeat(2, 1fr)", gap: 8 }}>
                {window.People.filter(p => !members.includes(p.id)).map((p, i) => (
                  <div key={p.id}
                    onClick={() => addMember(p.id)}
                    style={{
                      display: "flex", alignItems: "center", gap: 10,
                      padding: 8, border: "1px dashed var(--rs-line-2)", borderRadius: 2,
                      cursor: "pointer", background: "var(--rs-surface-2)",
                    }}>
                    <Avatar name={p.name} idx={i + 5} size={24} />
                    <div style={{ flex: 1 }}>
                      <div style={{ fontSize: 13 }}>{p.name}</div>
                      <div className="rs-mono-sm">{p.email}</div>
                    </div>
                    <span className="rs-mono-sm">+ add</span>
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

// ============================================================
// Reservation editor
// ============================================================

function ReservationEditor({ initialGroupId, onOpenSchedule }) {
  const [groupId, setGroupId] = useStateL(initialGroupId || window.DeviceGroups.filter(g => g.status === "Active")[0]?.id);
  const [testGroupId, setTestGroupId] = useStateL(window.TestGroups[0].id);
  const [date, setDate] = useStateL("2026-05-09");
  const [start, setStart] = useStateL("10:00");
  const [end, setEnd] = useStateL("13:00");
  const [notes, setNotes] = useStateL("");

  const dayOffset = (() => {
    const d = new Date(date + "T00:00");
    const base = new Date(2026, 4, 8);
    return Math.round((d - base) / 86400000);
  })();
  const startH = parseHM(start), endH = parseHM(end);

  // Conflict check
  const conflicts = useMemoL(() => {
    const out = [];
    window.Reservations.forEach(r => {
      if (r.dayOffset !== dayOffset) return;
      if (r.status !== "Confirmed") return;
      const overlap = startH < r.endHour && endH > r.startHour;
      if (!overlap) return;
      if (r.groupId === groupId) out.push({ rule: "R10", text: "Group already booked", r });
      if (r.testGroupId === testGroupId) out.push({ rule: "R11", text: "Test-group already booked", r });
    });
    return out;
  }, [groupId, testGroupId, dayOffset, startH, endH]);
  const validRange = endH > startH;
  const targetGroup = window.DeviceGroups.find(g => g.id === groupId);
  const groupActive = targetGroup?.status === "Active";

  return (
    <div className="rs-screen">
      <div className="rs-screen-head">
        <div>
          <div className="rs-screen-eyebrow">08 · Reservation Editor</div>
          <h1 className="rs-screen-title">New reservation</h1>
          <p className="rs-screen-sub rs-mono-sm">Pick a Device-Group, a Test-Group, and a time window. Conflicts are flagged inline.</p>
        </div>
        <div className="rs-screen-actions">
          <button className="rs-btn">Cancel</button>
          <button className="rs-btn">Save as Pending</button>
          <button className="rs-btn rs-btn--primary"
            disabled={conflicts.length > 0 || !validRange || !groupActive}>
            Confirm
          </button>
        </div>
      </div>

      <div className="rs-resv-editor">
        <div className="rs-resv-form-wrap">
          {conflicts.length > 0 && (
            <div className="rs-banner" style={{ marginBottom: 16 }}>
              <div style={{ flex: 1 }}>
                <span className="rs-banner-title">Conflict detected.</span>
                {conflicts.map((c, i) => {
                  const otherGroup = window.DeviceGroups.find(g => g.id === c.r.groupId);
                  const otherTeam = window.TestGroups.find(t => t.id === c.r.testGroupId);
                  return (
                    <div key={i} style={{ marginTop: 4 }}>
                      {c.text}: <a href="#" onClick={(e) => { e.preventDefault(); onOpenSchedule(); }}>
                        <span className="rs-mono">{c.r.id}</span></a> ·{" "}
                      <span className="rs-mono-sm">{otherGroup?.name} · {otherTeam?.name} · {window.fmtHourMono(c.r.startHour)}–{window.fmtHourMono(c.r.endHour)}</span>
                      <span className="rs-banner-rule">{c.rule}</span>
                    </div>
                  );
                })}
              </div>
            </div>
          )}
          {!validRange && (
            <div className="rs-banner" style={{ marginBottom: 16 }}>
              <div style={{ flex: 1 }}><span className="rs-banner-title">Invalid range.</span> End time must be after start.</div>
              <span className="rs-banner-rule">R13</span>
            </div>
          )}
          {!groupActive && (
            <div className="rs-banner" style={{ marginBottom: 16 }}>
              <div style={{ flex: 1 }}><span className="rs-banner-title">Group not active.</span> Activate it before confirming a reservation.</div>
              <span className="rs-banner-rule">R9</span>
            </div>
          )}

          <div className="rs-form">
            <div className="rs-field rs-field--full">
              <label className="rs-label">Device-Group</label>
              <select className="rs-select" value={groupId} onChange={(e) => setGroupId(e.target.value)}>
                {window.DeviceGroups.map(g => (
                  <option key={g.id} value={g.id}>{g.name} · {g.status}</option>
                ))}
              </select>
            </div>
            <div className="rs-field rs-field--full">
              <label className="rs-label">Test-Group</label>
              <select className="rs-select" value={testGroupId} onChange={(e) => setTestGroupId(e.target.value)}>
                {window.TestGroups.map(t => (
                  <option key={t.id} value={t.id}>{t.name} · {t.memberIds.length} people</option>
                ))}
              </select>
            </div>
            <div className="rs-field">
              <label className="rs-label">Date</label>
              <input type="date" className="rs-input" value={date} onChange={(e) => setDate(e.target.value)} />
            </div>
            <div className="rs-field"></div>
            <div className="rs-field">
              <label className="rs-label">Start (24h)</label>
              <input type="time" className="rs-input" value={start} onChange={(e) => setStart(e.target.value)} />
            </div>
            <div className="rs-field">
              <label className="rs-label">End (24h)</label>
              <input type="time" className="rs-input" value={end} onChange={(e) => setEnd(e.target.value)} />
            </div>
            <div className="rs-field rs-field--full">
              <label className="rs-label">Notes (optional)</label>
              <textarea className="rs-textarea" rows="3" value={notes} onChange={(e) => setNotes(e.target.value)}
                placeholder="What is this reservation for?" />
            </div>
          </div>
        </div>

        <div className="rs-resv-side">
          <div className="rs-pane-title" style={{ marginBottom: 12 }}>
            <span>Preview</span>
            <span className="rs-mono-sm">{window.dateForOffset(dayOffset).toLocaleDateString("en", { weekday: "short", month: "short", day: "numeric" })}</span>
          </div>
          <div className="rs-resv-preview" style={{ marginBottom: 16 }}>
            <ResvPreview groupId={groupId} testGroupId={testGroupId} dayOffset={dayOffset} startH={startH} endH={endH} conflicts={conflicts} />
          </div>

          <div className="rs-pane-title" style={{ marginBottom: 12 }}>
            <span>Group</span>
          </div>
          {targetGroup && (
            <div className="rs-resv-preview" style={{ marginBottom: 16 }}>
              <div style={{ fontWeight: 500, marginBottom: 4 }}>{targetGroup.name}</div>
              <div className="rs-mono-sm" style={{ marginBottom: 10 }}>{targetGroup.deviceIds.length} devices · {targetGroup.connections.length} connections</div>
              <div style={{ height: 100 }}><MiniTopology group={targetGroup} /></div>
              <div style={{ marginTop: 8 }}><Pill status={targetGroup.status} kind="group" /></div>
            </div>
          )}

          <div className="rs-pane-title" style={{ marginBottom: 12 }}><span>Team</span></div>
          {(() => {
            const t = window.TestGroups.find(x => x.id === testGroupId);
            const ppl = t.memberIds.map(id => window.People.find(p => p.id === id));
            return (
              <div className="rs-resv-preview">
                <div style={{ fontWeight: 500, marginBottom: 8 }}>{t.name}</div>
                <AvatarCluster people={ppl} max={6} />
              </div>
            );
          })()}
        </div>
      </div>
    </div>
  );
}

function parseHM(s) {
  const [h, m] = s.split(":").map(Number);
  return h + (m || 0) / 60;
}

function ResvPreview({ groupId, testGroupId, dayOffset, startH, endH, conflicts }) {
  const W = 320, H = 90;
  const PAD_L = 8;
  const START = 6, END = 22;
  const HOURS = END - START;
  const xH = (h) => PAD_L + ((h - START) / HOURS) * (W - 16);
  const others = window.Reservations.filter(r => r.dayOffset === dayOffset && r.status === "Confirmed");
  return (
    <svg viewBox={`0 0 ${W} ${H}`} width="100%" height={H} style={{ display: "block" }}>
      {Array.from({ length: HOURS + 1 }, (_, i) => {
        const h = START + i;
        const x = xH(h);
        return (
          <g key={i}>
            <line x1={x} y1="20" x2={x} y2={H - 8} stroke="var(--rs-line)" strokeDasharray={i % 4 === 0 ? "" : "2 4"} />
            {i % 4 === 0 && <text x={x + 2} y="14" fontFamily="IBM Plex Mono, ui-monospace, monospace" fontSize="9" fill="var(--rs-ink-4)">{String(h).padStart(2, "0")}</text>}
          </g>
        );
      })}
      <text x="6" y={42} fontFamily="IBM Plex Mono, ui-monospace, monospace" fontSize="9" fill="var(--rs-ink-4)">EXISTING</text>
      <text x="6" y={72} fontFamily="IBM Plex Mono, ui-monospace, monospace" fontSize="9" fill="var(--rs-ink-4)">NEW</text>
      {/* existing */}
      {others.filter(r => r.groupId === groupId || r.testGroupId === testGroupId).map(r => (
        <rect key={r.id} x={xH(r.startHour)} y={32} width={Math.max(2, xH(r.endHour) - xH(r.startHour))} height={14} rx="2"
          fill="var(--rs-ok)" stroke="var(--rs-ok)" />
      ))}
      {/* new */}
      <rect x={xH(startH)} y={62} width={Math.max(2, xH(endH) - xH(startH))} height={14} rx="2"
        fill={conflicts.length ? "var(--rs-bad-soft)" : "var(--rs-warn-soft)"}
        stroke={conflicts.length ? "var(--rs-bad)" : "var(--rs-warn)"} strokeWidth="1.25"
        strokeDasharray="4 3" />
    </svg>
  );
}

Object.assign(window, { Dashboard, DevicesList, DeviceGroupsList, BuildingsList, TestGroupsList, ReservationEditor });
