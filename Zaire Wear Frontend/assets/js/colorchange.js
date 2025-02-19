document.addEventListener("DOMContentLoaded", () => {
    const colorButtons = document.querySelectorAll(".color-options .color");

    colorButtons.forEach(button => {
        button.addEventListener("click", () => {
            colorButtons.forEach(btn => btn.classList.remove("selected"));
            button.classList.add("selected");

            const selectedColor = button.dataset.color;
            console.log("Selected color:", selectedColor);
        });
    });
});
