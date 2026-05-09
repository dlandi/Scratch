Design the UI for a web application called "Resource Scheduler."

Purpose: lab managers schedule shared physical test equipment.
Equipment is wired together into named "Device-Groups," and a
"Device-Group" is reserved as a single unit by a named "Test-Group"
(a small team of people).

Domain entities to surface:
- Building: name, mailing address, contains many Devices.
- Device: name, status (Available, Maintenance, Offline, Retired),
  located in one Building, optionally a member of one Device-Group.
- Device-Group: a named, connected set of Devices with a topology of
  cabled connections. Status is Active or Inactive.
- Person: name, email.
- Test-Group: a named team of People who book reservations.
- Reservation: a Device-Group booked by a Test-Group over a time
  window. Status is Pending, Confirmed, Cancelled, or Completed.

Screens to design:

1. Dashboard / overview
   Header with app name and primary nav (Buildings, Devices,
   Device-Groups, Test-Groups, Schedule). Cards for "Active groups
   today," "Upcoming reservations," "Devices needing attention."

2. Device-Group Designer (the marquee screen)
   A large SVG canvas in the center showing each member device as a
   labeled node and each connection as an edge. To the left, a
   filterable device picker (chips). To the right, a properties panel
   (group name, status toggle, validation messages). Node fill encodes
   device status; node border encodes membership state (solid = in
   this group, dashed = locked because it is already in another active
   group, none = free to add). Edges show optional cable labels. The
   canvas should feel like a small schematic, not a flowchart.

3. Schedule Timeline
   A horizontal SVG timeline spanning a day or a week, with one row
   per Device-Group. Each Reservation is a rounded SVG rectangle on
   its row, color-coded by status (Pending = outlined, Confirmed =
   filled, Cancelled = struck through, Completed = muted). Drag to
   create a new reservation; if the candidate range overlaps an
   existing Confirmed reservation on the same group or by the same
   Test-Group, draw a red conflict band. A left rail lists groups
   with a small device-count badge.

4. Devices list
   Filterable table: name, building, status (with a colored dot),
   current Device-Group (or "Unassigned"). Bulk filter by status and
   building.

5. Device-Groups list
   Card grid. Each card shows the group name, an SVG mini-thumbnail
   of its topology, status pill (Active / Inactive), member count,
   and next upcoming reservation.

6. Buildings list
   Simple table of name, address, device count. Click-through opens
   a building detail view that shows its devices.

7. Test-Groups
   List of test-groups with member avatars (SVG initial circles).
   Editor that adds and removes people.

8. Reservation editor
   Form to pick Device-Group, Test-Group, date, and time range.
   Inline conflict check that surfaces R10 (group overlap) and R11
   (test-group overlap) violations as a red banner with the
   conflicting reservation linked.

Visual style:
- Technical, clean, lab-instrument feel. Not playful.
- Structural visualizations (canvas, timeline, device chips, avatars)
  rendered in inline SVG so they scale and theme cleanly. Forms and
  tables are standard HTML.
- Light theme primary; reserve a dark variant for the timeline.
- Status palette: green = Available / Active / Confirmed,
  amber = Maintenance / Pending, slate-gray = Offline,
  near-black = Retired, red = conflict.
- Single shared stylesheet; class names prefixed "rs-".
- Use CSS custom properties for color and spacing tokens so the SVG
  fills and strokes pick them up via var(...).

Produce the Device-Group Designer screen first; it is the most
visually distinctive. Then the Schedule Timeline. Then the lists.