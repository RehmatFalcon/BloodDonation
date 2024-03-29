﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$.extend({
    getUrlVars: function () {
        var vars = [], hash;
        var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
        for (var i = 0; i < hashes.length; i++) {
            hash = hashes[i].split('=');
            vars.push(hash[0]);
            vars[hash[0]] = hash[1]
        }
        return vars
    }, getUrlVar: function (name) {
        return $.getUrlVars()[name]
    }
});
$('.pagination-previous').on('click', function () {
    $('.pagination-previous a').removeAttr('href');
    previousNext(!0)
});
$('.pagination-next').on('click', function () {
    $('.pagination-next a').removeAttr('href');
    previousNext(!1)
});
$('.pagination li>a').on('click', function () {
    $(this).removeAttr('href');
    const pageNum = this.dataset.page ?? this.textContent;
    navigate(pageNum)
});

function previousNext(is_previous) {
    var currentPage = parseInt($('.pagination li.current').find('span').remove().end().text());
    var new_page_num = currentPage + 1;
    if (is_previous == !0) {
        new_page_num = currentPage - 1
    }
    navigate(new_page_num)
}

function navigate(new_page_num) {
    const pageNumberQueryName = "page";
    var parsedUrl = window.location.href;
    var queryString = location.search;
    var urlParams = new URLSearchParams(location.search);
    if (queryString == "") {
        parsedUrl += `?${pageNumberQueryName}=` + new_page_num
    } else {
        if (urlParams.has(pageNumberQueryName)) {
            parsedUrl = parsedUrl.split('?')[0];
            urlParams.delete(pageNumberQueryName);
            parsedUrl += "?" + urlParams + `&${pageNumberQueryName}=` + new_page_num
        } else {
            parsedUrl += `&${pageNumberQueryName}=` + new_page_num
        }
    }
    window.location.href = parsedUrl
}

document.addEventListener("DOMContentLoaded", () => {
    const paginationLinks = document.querySelectorAll(".pagination li");
    paginationLinks.forEach(x => {
        x.classList.add('page-item');
        if (x.classList.contains("current")) {
            x.classList.add('active');
        }
        const link = x.querySelector("a")
        if (link) {
            link.classList.add('page-link');
        } else {
            const elm = document.createElement("a")
            elm.href = "#";
            elm.classList.add('page-link');
            x.removeChild(x.querySelector("span"));
            elm.textContent = x.textContent;
            x.innerHTML = "";
            x.appendChild(elm);
        }
    });
    const APP_HELPER = {};
    APP_HELPER.InitializeSingleTypeahead = (elem, config = {}, additionalPlugins = []) => {
        return new TomSelect(elem, {
            closeAfterSelect: true,
            allowEmptyOption: true,
            create: false,
            plugins: ['change_listener', 'dropdown_input', ...additionalPlugins],
            placeholder: elem.dataset.placeholder ?? "Select an option",
            render: {
                option: function (data, escape) {
                    const icon = data.icon ? `<i class="${data.icon}"></i>  ` : '';
                    return '<div>' +
                        '<span class="title">' + icon + escape(data.text) + '</span>' +
                        '</div>';
                },
                item: function (data, escape) {
                    const icon = data.icon ? `<i class="${data.icon}"></i>  ` : '';
                    return '<div>' +
                        '<span class="title">' + icon + escape(data.text) + '</span>' +
                        '</div>';
                },
                no_results: function (data, escape) {
                    return '<div class="no-results"> No results found for "' + escape(data.input) + '"</div>';
                },
            },
            ...config
        });
    };

    const elems = document.querySelectorAll('select');
    for (const elem of elems) {
        APP_HELPER.InitializeSingleTypeahead(elem);
    }

    const allReqAckElems = document.querySelectorAll(".req-ack");
    const askConfirm = (evt) => {
        const userAck = confirm(evt.target.dataset.ack ?? "Are you sure you want to perform this action?");
        if (!userAck) {
            evt.preventDefault();
        }
    };
    allReqAckElems.forEach(x => x.addEventListener("click",askConfirm));
});