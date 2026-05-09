// Seed data for Resource Scheduler prototype
// Domain: Building, Device, DeviceGroup, Person, TestGroup, Reservation

const Buildings = [
  { id: "b1", name: "Lab North",     address: "1400 Industrial Pkwy\nBuilding A · Floor 3\nCambridge, MA 02139", deviceCount: 0 },
  { id: "b2", name: "Lab South",     address: "210 Foundry Rd\nWest Wing · Bay 2\nCambridge, MA 02141",          deviceCount: 0 },
  { id: "b3", name: "Annex East",    address: "88 Pier Ave\nUnit 3-B\nSomerville, MA 02144",                      deviceCount: 0 },
  { id: "b4", name: "Storage Vault", address: "1400 Industrial Pkwy\nBasement Level\nCambridge, MA 02139",        deviceCount: 0 },
];

// Devices — short names like instrument labels
const Devices = [
  { id: "d01", name: "SCOPE-01",  status: "Available",   buildingId: "b1", groupId: "g1" },
  { id: "d02", name: "SCOPE-02",  status: "Available",   buildingId: "b1", groupId: "g2" },
  { id: "d03", name: "SCOPE-03",  status: "Maintenance", buildingId: "b1", groupId: null },
  { id: "d04", name: "AWG-01",    status: "Available",   buildingId: "b1", groupId: "g1" },
  { id: "d05", name: "AWG-02",    status: "Available",   buildingId: "b2", groupId: "g3" },
  { id: "d06", name: "DMM-01",    status: "Available",   buildingId: "b1", groupId: "g1" },
  { id: "d07", name: "DMM-02",    status: "Available",   buildingId: "b2", groupId: null },
  { id: "d08", name: "DMM-03",    status: "Offline",     buildingId: "b2", groupId: null },
  { id: "d09", name: "PSU-01",    status: "Available",   buildingId: "b1", groupId: "g1" },
  { id: "d10", name: "PSU-02",    status: "Available",   buildingId: "b2", groupId: "g3" },
  { id: "d11", name: "PSU-03",    status: "Available",   buildingId: "b3", groupId: null },
  { id: "d12", name: "LOAD-01",   status: "Available",   buildingId: "b1", groupId: null },
  { id: "d13", name: "LOAD-02",   status: "Available",   buildingId: "b2", groupId: "g3" },
  { id: "d14", name: "RFGEN-01",  status: "Available",   buildingId: "b3", groupId: "g4" },
  { id: "d15", name: "RFGEN-02",  status: "Maintenance", buildingId: "b3", groupId: null },
  { id: "d16", name: "SPEC-01",   status: "Available",   buildingId: "b3", groupId: "g4" },
  { id: "d17", name: "VNA-01",    status: "Available",   buildingId: "b3", groupId: null },
  { id: "d18", name: "TEMP-CHM-01", status: "Available", buildingId: "b1", groupId: "g2" },
  { id: "d19", name: "DAQ-01",    status: "Available",   buildingId: "b2", groupId: "g3" },
  { id: "d20", name: "DAQ-02",    status: "Available",   buildingId: "b2", groupId: null },
  { id: "d21", name: "PROBE-A",   status: "Available",   buildingId: "b1", groupId: "g2" },
  { id: "d22", name: "PROBE-B",   status: "Offline",     buildingId: "b1", groupId: null },
  { id: "d23", name: "REF-CLK",   status: "Available",   buildingId: "b3", groupId: "g4" },
  { id: "d24", name: "OLD-SCOPE", status: "Retired",     buildingId: "b4", groupId: null },
];

// Device groups, each with topology positions for the canvas (in normalized 0..1 viewport coords)
const DeviceGroups = [
  {
    id: "g1", name: "Bench A · Power Characterization", status: "Active",
    deviceIds: ["d09", "d04", "d06", "d01"],
    layout: { d09: [0.18, 0.62], d04: [0.42, 0.30], d06: [0.66, 0.62], d01: [0.84, 0.30] },
    connections: [
      { id: "c1", from: "d09", to: "d04", label: "DC-OUT" },
      { id: "c2", from: "d04", to: "d01", label: "CH-1" },
      { id: "c3", from: "d04", to: "d06", label: "TRIG" },
      { id: "c4", from: "d06", to: "d01", label: "CH-2" },
    ],
  },
  {
    id: "g2", name: "Bench B · Thermal Sweep", status: "Active",
    deviceIds: ["d02", "d18", "d21"],
    layout: { d02: [0.30, 0.32], d18: [0.55, 0.62], d21: [0.78, 0.32] },
    connections: [
      { id: "c5", from: "d18", to: "d02", label: "TEMP" },
      { id: "c6", from: "d18", to: "d21", label: "PROBE" },
    ],
  },
  {
    id: "g3", name: "Rack 4 · DAQ Cluster", status: "Active",
    deviceIds: ["d05", "d10", "d13", "d19"],
    layout: { d05: [0.22, 0.30], d10: [0.22, 0.68], d13: [0.55, 0.50], d19: [0.82, 0.50] },
    connections: [
      { id: "c7", from: "d05", to: "d13", label: "AC-IN" },
      { id: "c8", from: "d10", to: "d13", label: "DC-IN" },
      { id: "c9", from: "d13", to: "d19", label: "USB-3" },
    ],
  },
  {
    id: "g4", name: "RF Suite · 24 GHz", status: "Active",
    deviceIds: ["d14", "d16", "d23"],
    layout: { d14: [0.22, 0.40], d16: [0.62, 0.40], d23: [0.42, 0.78] },
    connections: [
      { id: "c10", from: "d14", to: "d16", label: "RF-OUT" },
      { id: "c11", from: "d23", to: "d14", label: "10MHz" },
      { id: "c12", from: "d23", to: "d16", label: "10MHz" },
    ],
  },
  {
    id: "g5", name: "Bench A · Draft (Q3 reconfig)", status: "Inactive",
    deviceIds: ["d11", "d12"],
    layout: { d11: [0.32, 0.45], d12: [0.66, 0.45] },
    connections: [{ id: "c13", from: "d11", to: "d12", label: "DC" }],
  },
];

// People
const People = [
  { id: "p01", name: "Aoife O'Brien",      email: "aoife@lab.example" },
  { id: "p02", name: "Dawit Bekele",       email: "dawit@lab.example" },
  { id: "p03", name: "Nadia Petrov",       email: "nadia@lab.example" },
  { id: "p04", name: "Ren Tanaka",         email: "ren@lab.example" },
  { id: "p05", name: "Mira Chandrasekar",  email: "mira@lab.example" },
  { id: "p06", name: "Joachim Hertz",      email: "joachim@lab.example" },
  { id: "p07", name: "Sasha Volkov",       email: "sasha@lab.example" },
  { id: "p08", name: "Kerry Ng",           email: "kerry@lab.example" },
  { id: "p09", name: "Felix Adebayo",      email: "felix@lab.example" },
  { id: "p10", name: "Imogen Walsh",       email: "imogen@lab.example" },
];

// Test groups
const TestGroups = [
  { id: "t1", name: "Power Team",   memberIds: ["p01", "p02", "p06"] },
  { id: "t2", name: "RF Team",      memberIds: ["p03", "p04", "p07", "p08"] },
  { id: "t3", name: "Thermal Team", memberIds: ["p05", "p09"] },
  { id: "t4", name: "DAQ Team",     memberIds: ["p02", "p10", "p06"] },
];

// Today is 2026-05-08 (anchor for the timeline). Reservations span the visible day/week.
// Hours stored as floats from start-of-day in local lab time for simplicity in the prototype.
// Each reservation: { id, groupId, testGroupId, dayOffset (0 = today), startHour, endHour, status, notes }
const Reservations = [
  // Today
  { id: "r01", groupId: "g1", testGroupId: "t1", dayOffset: 0, startHour: 9.0,  endHour: 12.0, status: "Confirmed", notes: "Production sweep — 1.2V → 3.3V rails." },
  { id: "r02", groupId: "g1", testGroupId: "t4", dayOffset: 0, startHour: 13.5, endHour: 17.0, status: "Pending",   notes: "DAQ regression run." },
  { id: "r03", groupId: "g2", testGroupId: "t3", dayOffset: 0, startHour: 8.0,  endHour: 11.5, status: "Confirmed", notes: "Thermal soak at 85°C." },
  { id: "r04", groupId: "g2", testGroupId: "t3", dayOffset: 0, startHour: 14.0, endHour: 16.0, status: "Cancelled", notes: "Rescheduled to Mon." },
  { id: "r05", groupId: "g3", testGroupId: "t4", dayOffset: 0, startHour: 10.0, endHour: 13.0, status: "Confirmed", notes: "" },
  { id: "r06", groupId: "g3", testGroupId: "t1", dayOffset: 0, startHour: 15.0, endHour: 18.0, status: "Pending",   notes: "Awaiting confirmation." },
  { id: "r07", groupId: "g4", testGroupId: "t2", dayOffset: 0, startHour: 9.5,  endHour: 14.0, status: "Confirmed", notes: "S-parameter sweep." },

  // Yesterday — completed
  { id: "r08", groupId: "g1", testGroupId: "t1", dayOffset: -1, startHour: 9.0,  endHour: 16.0, status: "Completed", notes: "" },
  { id: "r09", groupId: "g4", testGroupId: "t2", dayOffset: -1, startHour: 13.0, endHour: 17.0, status: "Completed", notes: "" },

  // Tomorrow
  { id: "r10", groupId: "g1", testGroupId: "t1", dayOffset: 1, startHour: 8.0,  endHour: 11.0, status: "Confirmed", notes: "" },
  { id: "r11", groupId: "g3", testGroupId: "t4", dayOffset: 1, startHour: 9.0,  endHour: 12.0, status: "Confirmed", notes: "" },
  { id: "r12", groupId: "g4", testGroupId: "t2", dayOffset: 1, startHour: 14.0, endHour: 18.0, status: "Pending",   notes: "" },
  { id: "r13", groupId: "g2", testGroupId: "t3", dayOffset: 1, startHour: 10.0, endHour: 13.0, status: "Confirmed", notes: "" },

  // Day after
  { id: "r14", groupId: "g1", testGroupId: "t4", dayOffset: 2, startHour: 9.0,  endHour: 14.0, status: "Confirmed", notes: "" },
  { id: "r15", groupId: "g3", testGroupId: "t1", dayOffset: 2, startHour: 13.0, endHour: 17.0, status: "Pending",   notes: "" },
];

// Compute building deviceCounts
Buildings.forEach(b => { b.deviceCount = Devices.filter(d => d.buildingId === b.id).length; });

// ----- Helpers -----
const STATUS_DEVICE   = ["Available", "Maintenance", "Offline", "Retired"];
const STATUS_GROUP    = ["Active", "Inactive"];
const STATUS_RESV     = ["Pending", "Confirmed", "Cancelled", "Completed"];

function deviceStatusToClass(s) {
  return ({ Available:"ok", Maintenance:"warn", Offline:"off", Retired:"retired" })[s] || "off";
}
function groupStatusToClass(s) {
  return s === "Active" ? "ok" : "off";
}
function resvStatusToClass(s) {
  return ({ Pending:"warn", Confirmed:"ok", Cancelled:"bad", Completed:"off" })[s] || "off";
}

// Initials, e.g. "Aoife O'Brien" -> "AO"
function initials(name) {
  const parts = name.split(/\s+/).filter(Boolean);
  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase();
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

function fmtHour(h) {
  const hh = Math.floor(h);
  const mm = Math.round((h - hh) * 60);
  const period = hh >= 12 ? "PM" : "AM";
  const h12 = hh === 0 ? 12 : (hh > 12 ? hh - 12 : hh);
  return `${h12}:${String(mm).padStart(2, "0")} ${period}`;
}

function fmtHourMono(h) {
  const hh = Math.floor(h);
  const mm = Math.round((h - hh) * 60);
  return `${String(hh).padStart(2,"0")}:${String(mm).padStart(2,"0")}`;
}

const DAY_LABELS = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
// Today (offset 0) = Friday 2026-05-08
function dateForOffset(off) {
  // anchor: Fri May 8, 2026
  const base = new Date(2026, 4, 8);
  const d = new Date(base.getTime() + off * 86400000);
  return d;
}

Object.assign(window, {
  Buildings, Devices, DeviceGroups, People, TestGroups, Reservations,
  STATUS_DEVICE, STATUS_GROUP, STATUS_RESV,
  deviceStatusToClass, groupStatusToClass, resvStatusToClass,
  initials, fmtHour, fmtHourMono, dateForOffset, DAY_LABELS,
});
