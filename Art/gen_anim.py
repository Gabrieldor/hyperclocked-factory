from PIL import Image

IDLE   = r'D:\Unity\Hyperclocked Factory\Art\SteamExtractor-idle.png'
HALTED = r'D:\Unity\Hyperclocked Factory\Art\SteamExtractor-halted.png'
OUT    = r'D:\Unity\Hyperclocked Factory\Art\AnimationSpreedSheet.png'

idle   = Image.open(IDLE).convert('RGBA')
halted = Image.open(HALTED).convert('RGBA')

# Status light colors
ACTIVE_LIGHT = (153, 229, 80, 255)   # #99E550 lime green
IDLE_LIGHT   = (153, 229, 80, 255)   # same as idle
# Status light pixel positions (bottom-right, 3x3 block at x=27-29, y=27-29)
STATUS_PX = [(x, y) for x in range(27, 30) for y in range(27, 30)]

def copy_frame(base):
    return base.copy()

def set_status(img, color):
    px = img.load()
    for (x, y) in STATUS_PX:
        px[x, y] = color
    return img

# Frame 0 — idle (as-is)
frame_idle = copy_frame(idle)

# Frame 1 — halted (as-is, already has red light)
frame_halted = copy_frame(halted)

# Frame 2 — selected: idle + yellow inner border (1px inside the 4px casing)
frame_selected = copy_frame(idle)
sel_px = frame_selected.load()
YELLOW = (251, 242, 54, 255)  # #FBF236
for x in range(4, 28):
    sel_px[x, 4]  = YELLOW
    sel_px[x, 27] = YELLOW
for y in range(4, 28):
    sel_px[4,  y] = YELLOW
    sel_px[27, y] = YELLOW

# Frames 3-9 — active: drill pumps down 0→1→2→3→2→1→0
# Shift inner region (x=5-26, y=5-26) downward, preserving casing and status light
idle_px = idle.load()

# Collect inner pixels to shift (excludes outer 4px casing and bottom status row)
INNER_X = range(5, 27)
INNER_Y = range(5, 27)

# Background fill color — sample from inner top edge (y=5 row, which is casing interior bg)
bg_color = idle_px[5, 5]

offsets = [0, 1, 2, 3, 2, 1, 0]  # 7 frames, pump stroke

active_frames = []
for offset in offsets:
    frame = copy_frame(idle)
    fpx = frame.load()

    if offset > 0:
        # Shift inner region down by offset rows
        # Read source pixels first (bottom-up to avoid overwrite)
        for y in reversed(INNER_Y):
            for x in INNER_X:
                src_y = y - offset
                if src_y >= 5:
                    fpx[x, y] = idle_px[x, src_y]
                else:
                    fpx[x, y] = bg_color  # fill vacated top rows

    # Active status light — lime green
    set_status(frame, ACTIVE_LIGHT)
    active_frames.append(frame)

# Assemble sprite sheet
all_frames = [frame_idle, frame_halted, frame_selected] + active_frames
sheet = Image.new('RGBA', (32 * len(all_frames), 32), (0, 0, 0, 0))
for i, f in enumerate(all_frames):
    sheet.paste(f, (i * 32, 0))

sheet.save(OUT)
print(f'Saved {len(all_frames)} frames -> {OUT}')
print('Frame order: 0=idle  1=halted  2=selected  3-9=active (7 frames)')
