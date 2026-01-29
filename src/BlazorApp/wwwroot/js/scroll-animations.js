// Scroll animations using Intersection Observer
function initScrollAnimations() {
    const observerOptions = {
        threshold: 0.05,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
                // Optional: stop observing after animation
                observer.unobserve(entry.target);
            }
        });
    }, observerOptions);

    // Observe all sections
    const sections = document.querySelectorAll('section');
    sections.forEach(section => {
        section.classList.add('animate-on-scroll');
        observer.observe(section);
    });

    // Observe cards and timeline items
    const animatedElements = document.querySelectorAll(
        '.project-card, .skill-category, .timeline-item, .about-card, .contact-form-card'
    );

    animatedElements.forEach((element, index) => {
        element.classList.add('animate-on-scroll');
        element.style.transitionDelay = `${(index % 6) * 0.1}s`;
        observer.observe(element);
    });
}

// Initialize on DOM load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initScrollAnimations);
} else {
    initScrollAnimations();
}

// Re-initialize after Blazor finishes rendering
setTimeout(initScrollAnimations, 500);
setTimeout(initScrollAnimations, 1500);
