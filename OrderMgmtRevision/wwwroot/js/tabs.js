document.addEventListener("DOMContentLoaded", function () {
    let tabs = JSON.parse(localStorage.getItem("openedTabs")) || [];
    let currentPage = { title: document.title, url: window.location.pathname };

    if (!tabs.some(tab => tab.url === currentPage.url)) {
        tabs.push(currentPage);
        localStorage.setItem("openedTabs", JSON.stringify(tabs));
    }

    let tabsContainer = document.getElementById("tabsContainer");
    if (tabsContainer) {
        tabsContainer.setAttribute("data-tabs", JSON.stringify(tabs));
        renderTabs();
    }
});

function renderTabs() {
    let tabsContainer = document.getElementById("tabsContainer");
    if (!tabsContainer) return;

    let tabs = JSON.parse(tabsContainer.getAttribute("data-tabs")) || [];

    tabsContainer.innerHTML = tabs.map(tab => {
        `<a href="${tab.url}" class="tab-item">
            ${tab.title}
            <i class="bi bi-x"></i>
        </a>`
        }).join("");
}

function removeTab() {
    
}
