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

export function setScrollLeft(el, x) {
    if (!el) return;
    el.scrollLeft = x;
}

/**
 * Position the element's horizontal scroll so that the given x coordinate
 * lands at `fraction` of the visible viewport width from the left. Useful
 * for keeping a NOW-marker near (but not at) the left edge of a timeline
 * after a re-render.
 */
export function scrollToShowAt(el, x, fraction) {
    if (!el) return;
    const w = el.clientWidth || 0;
    const target = x - w * (fraction || 0);
    el.scrollLeft = Math.max(0, target);
}
