const slides = document.querySelectorAll(".slide");
const slidesContainer = document.querySelector(".slides");
let slideIndex = 0;
let intervalId = null;

document.addEventListener("DOMContentLoaded", () => {
    showSlide(slideIndex);
    intervalId = setInterval(nextSlide, 5000);
});

function showSlide(index) {
    if (index >= slides.length) {
        slideIndex = 0;
    } else if (index < 0) {
        slideIndex = slides.length - 1;
    } else {
        slideIndex = index;
    }

    const offset = -slideIndex * 100 + "%";
    slidesContainer.style.transform = `translateX(${offset})`;
}

function prevSlide() {
    clearInterval(intervalId);
    showSlide(slideIndex - 1);
    resetTimer();
}

function nextSlide() {
    showSlide(slideIndex + 1);
    resetTimer();
}

function resetTimer() {
    clearInterval(intervalId);
    intervalId = setInterval(nextSlide, 5000);
}
