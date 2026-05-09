/* global React, ReactDOM */
const { useState: useStateA, useMemo: useMemoA } = React;

const TWEAK_DEFAULTS = window.TWEAK_DEFAULTS;

// ---- Tweak → token maps ----
const BG_PALETTES = {
  warm:  { "--rs-bg": "#f5f4f1", "--rs-surface": "#fbfaf7", "--rs-surface-2": "#ffffff",
           "--rs-line": "#e3e0d8", "--rs-line-2": "#d3cfc4", "--rs-line-3": "#bdb8aa" },
  cool:  { "--rs-bg": "#eef0f3", "--rs-surface": "#f7f8fa", "--rs-surface-2": "#ffffff",
           "--rs-line": "#dde0e6", "--rs-line-2": "#cdd1d8", "--rs-line-3": "#b3b8c2" },
  paper: { "--rs-bg": "#fafafa", "--rs-surface": "#ffffff", "--rs-surface-2": "#ffffff",
           "--rs-line": "#ebebeb", "--rs-line-2": "#d9d9d9", "--rs-line-3": "#bcbcbc" },
};

const ACCENT_OPTIONS = ["#2a4d6b", "#2c5b3f", "#7a3a8c", "#8a5a18"];
const ACCENT_SOFT = {
  "#2a4d6b": "#d6e1ec",
  "#2c5b3f": "#d8e6dc",
  "#7a3a8c": "#e6d6ea",
  "#8a5a18": "#ecdfc6",
};

const STATUS_VARIANTS = {
  instrument: {
    "--rs-ok": "#2e7d49", "--rs-ok-soft": "#d8ead9", "--rs-ok-line": "#99c4a4",
    "--rs-warn": "#b06a00", "--rs-warn-soft": "#f3e2c2", "--rs-warn-line": "#d4ad62",
    "--rs-off": "#5b6470", "--rs-off-soft": "#dde0e4", "--rs-off-line": "#aab1bb",
    "--rs-bad": "#b1281e", "--rs-bad-soft": "#f4d7d3", "--rs-bad-line": "#d8857d",
  },
  vivid: {
    "--rs-ok": "#1f9d4d", "--rs-ok-soft": "#cdedd4", "--rs-ok-line": "#7ec48f",
    "--rs-warn": "#d68000", "--rs-warn-soft": "#fbe5b8", "--rs-warn-line": "#e7b558",
    "--rs-off": "#4a5260", "--rs-off-soft": "#d6dae0", "--rs-off-line": "#9aa1ad",
    "--rs-bad": "#d6291c", "--rs-bad-soft": "#fad2cd", "--rs-bad-line": "#e58a82",
  },
};

const DENSITY_TOKENS = {
  compact: { "--rs-s-3": "8px", "--rs-s-4": "12px", "--rs-s-5": "18px",
             "--rs-s-6": "22px", "--rs-s-7": "32px",
             "--rs-row-h": "32px", "--rs-row-h-tl": "40px", "--rs-pad-l": "16px" },
  regular: { "--rs-s-3": "12px", "--rs-s-4": "16px", "--rs-s-5": "24px",
             "--rs-s-6": "32px", "--rs-s-7": "48px",
             "--rs-row-h": "44px", "--rs-row-h-tl": "48px", "--rs-pad-l": "24px" },
  comfy:   { "--rs-s-3": "14px", "--rs-s-4": "20px", "--rs-s-5": "30px",
             "--rs-s-6": "40px", "--rs-s-7": "60px",
             "--rs-row-h": "52px", "--rs-row-h-tl": "56px", "--rs-pad-l": "28px" },
};

const FONT_PAIRS = {
  plex:    { sans: '"IBM Plex Sans", system-ui, sans-serif', mono: '"IBM Plex Mono", ui-monospace, monospace' },
  jetbrain:{ sans: '"Inter Tight", system-ui, sans-serif', mono: '"JetBrains Mono", ui-monospace, monospace' },
  pragmata:{ sans: '"DM Sans", system-ui, sans-serif',     mono: '"DM Mono", ui-monospace, monospace' },
};

function App() {
  const [screen, setScreen] = useStateA("designer");
  const [designerGroup, setDesignerGroup] = useStateA("g1");
  const [resvGroup, setResvGroup] = useStateA(null);
  const [t, setTweak] = useTweaks(TWEAK_DEFAULTS);

  const onNav = (s) => setScreen(s);
  const onOpenDesigner = (gid) => { setDesignerGroup(gid || "g1"); setScreen("designer"); };
  const onNewReservation = (gid) => { setResvGroup(gid); setScreen("reservation"); };
  const onOpenSchedule = () => setScreen("schedule");

  // Build root style from tweaks
  const rootStyle = useMemoA(() => {
    const fonts = FONT_PAIRS[t.fonts] || FONT_PAIRS.plex;
    return {
      ...BG_PALETTES[t.background],
      ...DENSITY_TOKENS[t.density],
      ...STATUS_VARIANTS[t.status],
      "--rs-accent": t.accent,
      "--rs-accent-soft": ACCENT_SOFT[t.accent] || "#d6e1ec",
      "--rs-font-sans": fonts.sans,
      "--rs-font-mono": fonts.mono,
    };
  }, [t]);

  let body;
  switch (screen) {
    case "dashboard":   body = <Dashboard onNav={onNav} />; break;
    case "buildings":   body = <BuildingsList />; break;
    case "devices":     body = <DevicesList />; break;
    case "groups":      body = <DeviceGroupsList onOpenDesigner={onOpenDesigner} />; break;
    case "designer":    body = <DeviceGroupDesigner key={designerGroup} initialGroupId={designerGroup} onOpenSchedule={onOpenSchedule} tweaks={t} />; break;
    case "tests":       body = <TestGroupsList />; break;
    case "schedule":    body = <ScheduleTimeline onNewReservation={() => onNewReservation()} tweaks={t} />; break;
    case "reservation": body = <ReservationEditor initialGroupId={resvGroup} onOpenSchedule={onOpenSchedule} />; break;
    default:            body = <Dashboard onNav={onNav} />;
  }

  return (
    <div className={`rs-app rs-density-${t.density} ${t.dotGrid ? "rs-dotgrid" : "rs-no-dotgrid"} ${t.confirmedFilled ? "rs-cf-filled" : "rs-cf-outlined"}`}
         style={rootStyle}>
      <Header screen={screen} setScreen={setScreen} />
      <div className="rs-main">{body}</div>

      <TweaksPanel title="Tweaks">
        <TweakSection label="Surface" />
        <TweakRadio label="Background" value={t.background}
          options={["warm", "cool", "paper"]}
          onChange={(v) => setTweak("background", v)} />
        <TweakColor label="Accent" value={t.accent}
          options={ACCENT_OPTIONS}
          onChange={(v) => setTweak("accent", v)} />
        <TweakRadio label="Status palette" value={t.status}
          options={["instrument", "vivid"]}
          onChange={(v) => setTweak("status", v)} />

        <TweakSection label="Layout" />
        <TweakRadio label="Density" value={t.density}
          options={["compact", "regular", "comfy"]}
          onChange={(v) => setTweak("density", v)} />
        <TweakSelect label="Type pairing" value={t.fonts}
          options={[
            { value: "plex",     label: "IBM Plex (default)" },
            { value: "jetbrain", label: "Inter Tight + JetBrains" },
            { value: "pragmata", label: "DM Sans + DM Mono" },
          ]}
          onChange={(v) => setTweak("fonts", v)} />

        <TweakSection label="Designer canvas" />
        <TweakToggle label="Schematic dot grid" value={t.dotGrid}
          onChange={(v) => setTweak("dotGrid", v)} />
        <TweakRadio label="Node shape" value={t.nodeShape}
          options={["module", "chip", "tag"]}
          onChange={(v) => setTweak("nodeShape", v)} />

        <TweakSection label="Schedule" />
        <TweakToggle label="Confirmed blocks filled" value={t.confirmedFilled}
          onChange={(v) => setTweak("confirmedFilled", v)} />
        <TweakSlider label="Hour-column width" value={t.hourW}
          min={48} max={120} step={2} unit="px"
          onChange={(v) => setTweak("hourW", v)} />
      </TweaksPanel>
    </div>
  );
}

const root = ReactDOM.createRoot(document.getElementById("app"));
root.render(<App />);
