export function scrollContainerTo(element, top) {
  if (!element) return;
  try {
    element.scrollTo({ top, behavior: 'smooth' });
  } catch {
    element.scrollTop = top;
  }
}
