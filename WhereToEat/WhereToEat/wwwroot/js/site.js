const divEl = document.querySelector("div[id ^= 'badgeCategories']");
const restaurantId = parseInt(divEl.id.split('_')[1]);
const addCatDivEl = document.getElementById('addCategory');


function getCategoryList() {
    let data = new FormData();
    data.append('restaurantId', restaurantId);
    emptyDiv();

    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', receivedCategories)
    xhr.open('POST', '../GetCategories', true);
    xhr.send(data);
}

function emptyDiv() {
    while (divEl.firstChild) {
        divEl.removeChild(divEl.lastChild);
    };
}

function receivedCategories() {
    let response = this.responseText
    console.log(response);
    let categories = JSON.parse(response);

    for (let i = 0; i < categories.length; i++) {
        const category = categories[i];
        const spanEL = document.createElement('span');
        spanEL.setAttribute('class', 'badge badge-light');
        const aEL = document.createElement('a');
        aEL.textContent = category.text;
        const closeSpanEL = document.createElement('span');
        closeSpanEL.innerHTML = " &#215;";
        closeSpanEL.setAttribute("class", "removeCat");

        closeSpanEL.addEventListener('click', () => { removeCategory(category.id) });

        spanEL.appendChild(aEL);
        spanEL.appendChild(closeSpanEL);

        divEl.appendChild(spanEL);
    }
}

function removeCategory(category) {
    let data = new FormData();
    data.append('restaurantId', restaurantId);
    data.append('categoryId', category)

    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', getCategoryList);
    xhr.addEventListener('load', createAddCategory);
    xhr.open('POST', '../RemoveCategory', true);
    xhr.send(data);
}


function createAddCategory() {
    let data = new FormData();
    data.append('restaurantId', restaurantId);

    const xhr = new XMLHttpRequest();
    xhr.addEventListener('load', allCategories)
    xhr.open('POST', '../GetCategoriesForRestaurant', true);
    xhr.send(data);
}

function allCategories() {
    var addCategoryDiv = document.getElementById('addCategory');
    var dropDown = document.getElementById('categories');
    while (dropDown.firstChild) {
        dropDown.removeChild(dropDown.lastChild);
    }

    let categories = JSON.parse(this.responseText);

    if (categories.length == 0) {
        addCategoryDiv.setAttribute("style", "display: none");
    }
    else {
        addCategoryDiv.setAttribute("style", "display: unset");
    }

    for (let i = 0; i < categories.length; i++) {
        let category = document.createElement('option');
        category.setAttribute('value', categories[i].id);
        category.innerText = categories[i].text;
        dropDown.appendChild(category);
    }

    let button = document.getElementById('addCategoryToRest');
    let newButton = button.cloneNode(true);
    button.parentNode.replaceChild(newButton, button);

    newButton.addEventListener('click', () => {
        let categoryToAdd = document.getElementById('categories');
        let categoryId = categoryToAdd.value;

        addCategoryToRest(categoryId);
    });
}

function addCategoryToRest(categoryId) {
    let data = new FormData();
    data.append('restaurantId', restaurantId);
    data.append('categoryId', categoryId);

    const xhr = new XMLHttpRequest();

    xhr.addEventListener('load', getCategoryList);
    xhr.addEventListener('load', createAddCategory);
    xhr.open('POST', '../AddCategoryToRestaurant', true);
    xhr.send(data);
}

getCategoryList();
createAddCategory();
