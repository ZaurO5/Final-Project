//document.addEventListener("DOMContentLoaded", () => {
//    const handleSelection = (selector, className, parentSelectors) => {
//        document.querySelectorAll(selector).forEach((options) => {
//            options.addEventListener("click", (event) => {
//                const button = event.target;
//                if (!button.classList.contains(className)) return;

//                const product = parentSelectors.map(sel => button.closest(sel)).find(el => el);
//                if (product) {
//                    product.querySelectorAll(`.${className}`).forEach((item) => {
//                        item.classList.remove("selected");
//                    });
//                    button.classList.add("selected");
//                }
//            });
//        });
//    };

//    handleSelection(".size-options", "size", [".product-card"]);
//    handleSelection(".color-options", "color", [".basket-wrapper", ".product-details"]);
//});
