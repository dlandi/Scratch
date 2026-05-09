export function pointToSvg(svgEl, clientX, clientY) {
    if (!svgEl || !svgEl.createSVGPoint) return { x: 0, y: 0 };
    const pt = svgEl.createSVGPoint();
    pt.x = clientX; pt.y = clientY;
    const ctm = svgEl.getScreenCTM();
    if (!ctm) return { x: 0, y: 0 };
    const inv = ctm.inverse();
    const transformed = pt.matrixTransform(inv);
    return { x: transformed.x, y: transformed.y };
}

export function getRect(el) {
    if (!el || !el.getBoundingClientRect) return { x: 0, y: 0, width: 0, height: 0 };
    const r = el.getBoundingClientRect();
    return { x: r.x, y: r.y, width: r.width, height: r.height };
}
