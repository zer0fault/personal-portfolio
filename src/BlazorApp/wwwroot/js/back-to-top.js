window.backToTopButton = {
    dotNetRef: null,
    scrollThreshold: 300,

    init: function (dotNetReference) {
        this.dotNetRef = dotNetReference;
        window.addEventListener('scroll', this.handleScroll);
    },

    handleScroll: function () {
        const scrollPosition = window.pageYOffset || document.documentElement.scrollTop;
        const isVisible = scrollPosition > window.backToTopButton.scrollThreshold;

        if (window.backToTopButton.dotNetRef) {
            window.backToTopButton.dotNetRef.invokeMethodAsync('UpdateVisibility', isVisible);
        }
    },

    scrollToTop: function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    },

    cleanup: function () {
        window.removeEventListener('scroll', this.handleScroll);
        this.dotNetRef = null;
    }
};
