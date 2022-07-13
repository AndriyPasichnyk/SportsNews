"use strict";
/**
 * @license
 * Copyright 2019 Google LLC. All Rights Reserved.
 * SPDX-License-Identifier: Apache-2.0
 */
Object.defineProperty(exports, "__esModule", { value: true });
// @ts-nocheck TODO remove when fixed
var map;
var marker;
var geocoder;
var responseDiv;
var response;
function initMap() {
    map = new google.maps.Map(document.getElementById("map"), {
        zoom: 8,
        center: { lat: -34.397, lng: 150.644 },
        mapTypeControl: false,
    });
    geocoder = new google.maps.Geocoder();
    var inputText = document.createElement("input");
    inputText.type = "text";
    inputText.placeholder = "Enter a location";
    var submitButton = document.createElement("input");
    submitButton.type = "button";
    submitButton.value = "Geocode";
    submitButton.classList.add("button", "button-primary");
    var clearButton = document.createElement("input");
    clearButton.type = "button";
    clearButton.value = "Clear";
    clearButton.classList.add("button", "button-secondary");
    response = document.createElement("pre");
    response.id = "response";
    response.innerText = "";
    responseDiv = document.createElement("div");
    responseDiv.id = "response-container";
    responseDiv.appendChild(response);
    var instructionsElement = document.createElement("p");
    instructionsElement.id = "instructions";
    //instructionsElement.innerHTML = "<strong>Instructions</strong>: Enter an address in the textbox to geocode or click on the map to reverse geocode.";
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(inputText);
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(submitButton);
    //map.controls[google.maps.ControlPosition.TOP_LEFT].push(clearButton);
    //map.controls[google.maps.ControlPosition.LEFT_TOP].push(instructionsElement);
    map.controls[google.maps.ControlPosition.LEFT_TOP].push(responseDiv);
    marker = new google.maps.Marker({
        map: map,
    });
    map.addListener("click", function (e) {
        geocode({ location: e.latLng });
    });
    submitButton.addEventListener("click", function () {
        return geocode({ address: inputText.value });
    });
    clearButton.addEventListener("click", function () {
        clear();
    });
    clear();
}
function clear() {
    marker.setMap(null);
    responseDiv.style.display = "none";
}
function geocode(request) {
    clear();
    geocoder
        .geocode(request)
        .then(function (result) {
        var results = result.results;
        map.setCenter(results[0].geometry.location);
        marker.setPosition(results[0].geometry.location);
        marker.setMap(map);
        responseDiv.style.display = "block";
        response.innerText = JSON.stringify(result, null, 2);
        document.getElementById("location-name").value = results[0].formatted_address;
        return results;
    })
        .catch(function (e) {
        alert("Geocode was not successful for the following reason: " + e);
    });
}
window.initMap = initMap;
//# sourceMappingURL=index.js.map