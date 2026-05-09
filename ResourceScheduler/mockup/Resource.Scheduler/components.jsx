/* global React */
const { useState, useMemo, useRef, useEffect, useCallback } = React;

// ===== Shared SVG primitives =====

function StatusDot({ status, size = 8 }) {
  const cls = window.deviceStatusToClass(status);
  return <span className={`rs-dot rs-dot--${cls}`} style={{ width: size, height: size }} />;
}

function Pill({ status, kind = "device", children }) {
  let cls;
  if (kind === "device") cls = window.deviceStatusToClass(status);
  else if (kind === "group") cls = window.groupStatusToClass(status);
  else if (kind === "resv") cls = window.resvStatusToClass(status);
  else cls = "off";
  return (
    <span className={`rs-pill rs-pill--${cls}`}>
      <span className="dot" />
      {children || status}
    </span>
  );
}

// SVG initial-circle avatar
function Avatar({ name, size = 24, idx = 0 }) {
  const ini = window.initials(name);
  // Hue rotation by idx for slight differentiation; keeps low chroma to stay neutral
  const hues = [210, 30, 160, 280, 90, 340, 50];
  const h = hues[idx % hues.length];
  const fill = `oklch(0.92 0.04 ${h})`;
  const stroke = `oklch(0.55 0.04 ${h})`;
  const text = `oklch(0.30 0.04 ${h})`;
  return (
    <svg width={size} height={size} viewBox="0 0 24 24" style={{ display: "inline-block" }}>
      <circle cx="12" cy="12" r="11" fill={fill} stroke={stroke} strokeWidth="1" />
      <text x="12" y="13" textAnchor="middle" dominantBaseline="middle"
        fontFamily="IBM Plex Mono, ui-monospace, monospace"
        fontSize="9" fontWeight="500" fill={text}>{ini}</text>
    </svg>
  );
}

function AvatarCluster({ people, max = 4 }) {
  const shown = people.slice(0, max);
  const rest = people.length - shown.length;
  return (
    <span className="rs-avatar-cluster">
      {shown.map((p, i) => <Avatar key={p.id} name={p.name} idx={i} />)}
      {rest > 0 && (
        <span className="rs-mono" style={{ marginLeft: 8, color: "var(--rs-ink-3)" }}>+{rest}</span>
      )}
    </span>
  );
}

// Mini topology thumbnail SVG, scales the group's layout into a small box
function MiniTopology({ group, width = "100%", height = "100%" }) {
  const devicesById = useMemo(
    () => Object.fromEntries(window.Devices.map(d => [d.id, d])),
    []
  );
  const W = 280, H = 110;
  const pad = 18;
  return (
    <svg viewBox={`0 0 ${W} ${H}`} width={width} height={height} preserveAspectRatio="xMidYMid meet">
      {/* edges */}
      {group.connections.map(c => {
        const a = group.layout[c.from], b = group.layout[c.to];
        if (!a || !b) return null;
        const ax = pad + a[0] * (W - 2 * pad);
        const ay = pad + a[1] * (H - 2 * pad);
        const bx = pad + b[0] * (W - 2 * pad);
        const by = pad + b[1] * (H - 2 * pad);
        return <line key={c.id} x1={ax} y1={ay} x2={bx} y2={by}
          stroke="var(--rs-ink-3)" strokeWidth="1.25" />;
      })}
      {/* nodes */}
      {group.deviceIds.map(id => {
        const d = devicesById[id];
        const p = group.layout[id];
        if (!d || !p) return null;
        const x = pad + p[0] * (W - 2 * pad);
        const y = pad + p[1] * (H - 2 * pad);
        const cls = window.deviceStatusToClass(d.status);
        return (
          <g key={id}>
            <rect x={x - 14} y={y - 8} width="28" height="16" rx="2"
              fill={`var(--rs-${cls === "ok" ? "ok-soft" : cls === "warn" ? "warn-soft" : cls === "off" ? "off-soft" : "retired-soft"})`}
              stroke={`var(--rs-${cls})`} strokeWidth="1" />
          </g>
        );
      })}
    </svg>
  );
}

// ===== Header / Nav =====

function Header({ screen, setScreen, lab }) {
  const items = [
    { id: "dashboard",   label: "Dashboard",      num: "01" },
    { id: "buildings",   label: "Buildings",      num: "02" },
    { id: "devices",     label: "Devices",        num: "03" },
    { id: "groups",      label: "Device-Groups",  num: "04" },
    { id: "designer",    label: "Designer",       num: "05" },
    { id: "tests",       label: "Test-Groups",    num: "06" },
    { id: "schedule",    label: "Schedule",       num: "07" },
    { id: "reservation", label: "New Reservation", num: "08" },
  ];
  return (
    <header className="rs-header">
      <div className="rs-brand">
        <svg className="rs-brand-mark" viewBox="0 0 22 22">
          <rect x="1.5" y="1.5" width="19" height="19" rx="2" fill="none"
            stroke="var(--rs-ink)" strokeWidth="1.25" />
          <circle cx="6"  cy="6"  r="2" fill="var(--rs-ok)" />
          <circle cx="16" cy="6"  r="2" fill="var(--rs-ink)" />
          <circle cx="6"  cy="16" r="2" fill="var(--rs-ink)" />
          <circle cx="16" cy="16" r="2" fill="var(--rs-warn)" />
          <line x1="6" y1="6" x2="16" y2="6" stroke="var(--rs-ink)" strokeWidth="1" />
          <line x1="6" y1="6" x2="6" y2="16" stroke="var(--rs-ink)" strokeWidth="1" />
          <line x1="16" y1="6" x2="16" y2="16" stroke="var(--rs-ink)" strokeWidth="1" />
          <line x1="6" y1="16" x2="16" y2="16" stroke="var(--rs-ink)" strokeWidth="1" />
        </svg>
        <span className="rs-brand-name"><b>Resource</b><span>.</span><b>Scheduler</b></span>
      </div>
      <nav className="rs-nav">
        {items.map(it => (
          <button key={it.id}
            className={`rs-nav-btn ${screen === it.id ? "is-active" : ""}`}
            onClick={() => setScreen(it.id)}>
            <span className="num">{it.num}</span>
            <span>{it.label}</span>
          </button>
        ))}
      </nav>
      <div className="rs-header-right">
        <span><span className="dot" />in-memory store</span>
        <span>·</span>
        <span>v0.1</span>
      </div>
    </header>
  );
}

Object.assign(window, { StatusDot, Pill, Avatar, AvatarCluster, MiniTopology, Header });
