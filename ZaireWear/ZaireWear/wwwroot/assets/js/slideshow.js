const slides = document.querySelectorAll(".slide");
const slidesContainer = document.querySelector(".slides");
const titleElement = document.getElementById("sliderTitle");
const subtitleElement = document.getElementById("sliderSubtitle");
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

    const currentSlide = slides[slideIndex];

    console.log("Current Slide:", currentSlide);
    console.log("Title:", currentSlide.getAttribute("data-title"));
    console.log("Subtitle:", currentSlide.getAttribute("data-subtitle"));

    titleElement.textContent = currentSlide.getAttribute("data-title");
    subtitleElement.textContent = currentSlide.getAttribute("data-subtitle");
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
