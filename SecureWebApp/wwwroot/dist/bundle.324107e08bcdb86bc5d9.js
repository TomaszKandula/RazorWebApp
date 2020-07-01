(window["webpackJsonp"] = window["webpackJsonp"] || []).push([["bundle"],{

/***/ "./node_modules/css-loader/dist/cjs.js!./node_modules/sass-loader/dist/cjs.js!./styles/site.scss":
/*!*******************************************************************************************************!*\
  !*** ./node_modules/css-loader/dist/cjs.js!./node_modules/sass-loader/dist/cjs.js!./styles/site.scss ***!
  \*******************************************************************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("exports = module.exports = __webpack_require__(/*! ../node_modules/css-loader/dist/runtime/api.js */ \"./node_modules/css-loader/dist/runtime/api.js\")(false);\n// Module\nexports.push([module.i, \"ol {\\n  list-style-type: none;\\n  counter-reset: item;\\n  margin: 0;\\n  padding: 0; }\\n\\nol > li {\\n  display: table;\\n  counter-increment: item;\\n  margin-bottom: 0.6em; }\\n\\nol > li:before {\\n  content: counters(item, \\\".\\\") \\\". \\\";\\n  display: table-cell;\\n  padding-right: 0.6em; }\\n\\nli ol > li {\\n  margin: 0; }\\n\\nli ol > li:before {\\n  content: counters(item, \\\".\\\") \\\" \\\"; }\\n\\nnav.navbar {\\n  height: 6rem !important;\\n  box-shadow: 0 1px 3px 0 rgba(0, 0, 0, 0.1), 0 1px 2px 0 rgba(0, 0, 0, 0.06) !important; }\\n\", \"\"]);\n\n\n\n//# sourceURL=webpack:///./styles/site.scss?./node_modules/css-loader/dist/cjs.js!./node_modules/sass-loader/dist/cjs.js");

/***/ }),

/***/ "./scripts/functions/ajax.js":
/*!***********************************!*\
  !*** ./scripts/functions/ajax.js ***!
  \***********************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _helpers__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./helpers */ \"./scripts/functions/helpers.js\");\n\r\n\r\n\r\n\r\n\r\n\n\n//# sourceURL=webpack:///./scripts/functions/ajax.js?");

/***/ }),

/***/ "./scripts/functions/helpers.js":
/*!**************************************!*\
  !*** ./scripts/functions/helpers.js ***!
  \**************************************/
/*! exports provided: FormatPhoneNumber, HasSpecialChar, HasLowerCase, HasUpperCase, IsNumeric, IsEmpty, ValidateEmail, CleanBaseUrl */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"FormatPhoneNumber\", function() { return FormatPhoneNumber; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"HasSpecialChar\", function() { return HasSpecialChar; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"HasLowerCase\", function() { return HasLowerCase; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"HasUpperCase\", function() { return HasUpperCase; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"IsNumeric\", function() { return IsNumeric; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"IsEmpty\", function() { return IsEmpty; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"ValidateEmail\", function() { return ValidateEmail; });\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"CleanBaseUrl\", function() { return CleanBaseUrl; });\n﻿\r\n\r\n\r\nfunction IsEmpty(value)\r\n{\r\n    return typeof value === 'string' && !value.trim() || typeof value === undefined || value === null;\r\n};\r\n\r\n\r\nfunction IsNumeric(n)\r\n{\r\n    return !isNaN(parseFloat(n)) && isFinite(n);\r\n};\r\n\r\n\r\nfunction ValidateEmail(email)\r\n{\r\n    var re = /\\S+@\\S+\\.\\S+/;\r\n    return re.test(email);\r\n};\r\n\r\n\r\nfunction FormatPhoneNumber(Number)\r\n{\r\n    Number = Number.replace(/[^\\d]+/g, '').replace(/(\\d{2})(\\d{3})(\\d{3})(\\d{3})/, '($1) $2 $3 $4');\r\n\r\n    if (isEmpty(Number))\r\n    {\r\n        return false;\r\n    }\r\n    else\r\n    {\r\n        return Number;\r\n    };\r\n\r\n};\r\n\r\n\r\nfunction HasLowerCase(str)\r\n{\r\n    if (str.toUpperCase() != str)\r\n    {\r\n        return true;\r\n    }\r\n\r\n    return false;\r\n}\r\n\r\n\r\nfunction HasUpperCase(str)\r\n{\r\n    if (str.toLowerCase() != str)\r\n    {\r\n        return true;\r\n    }\r\n\r\n    return false;\r\n}\r\n\r\n\r\nfunction HasSpecialChar(str)\r\n{\r\n    var format = /[!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]/;\r\n\r\n    if (format.test(str))\r\n    {\r\n        return true;\r\n    }\r\n    else\r\n    {\r\n        return false;\r\n    }\r\n\r\n}\r\n\r\nfunction CleanBaseUrl()\r\n{\r\n    var CurrentUrl = window.location.href;\r\n    var Check = 0;\r\n    var BaseUrl = \"\";\r\n\r\n    for (var iCNT = 0; iCNT <= CurrentUrl.length; iCNT++)\r\n    {\r\n        BaseUrl = CurrentUrl.charAt(iCNT);\r\n        if (BaseUrl.charAt(iCNT) === \"/\")\r\n        {\r\n            Check++;\r\n            if (Check === 2)\r\n            {\r\n                break;\r\n            };\r\n        };\r\n    }\r\n\r\n    return BaseUrl;\r\n\r\n}\r\n\r\n\r\n\r\n\n\n//# sourceURL=webpack:///./scripts/functions/helpers.js?");

/***/ }),

/***/ "./scripts/functions/modals.js":
/*!*************************************!*\
  !*** ./scripts/functions/modals.js ***!
  \*************************************/
/*! exports provided: default */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony export (binding) */ __webpack_require__.d(__webpack_exports__, \"default\", function() { return CallModalWindow; });\n﻿\r\n\r\n\r\nfunction CallModalWindow(ContentUrl, ModalText, ModalWindow)\r\n{\r\n\r\n    var Url = window.location.origin + ContentUrl;\r\n    var Request = new XMLHttpRequest();\r\n\r\n    Request.open('GET', Url, true);\r\n    Request.onload = function ()\r\n    {\r\n\r\n        if (this.status >= 200 && this.status < 400)\r\n        {\r\n\r\n            var Response = this.response;\r\n\r\n            var GetModalWindow = document.querySelector(\"#\" + ModalWindow);\r\n            var GetModalText   = document.querySelector(\"#\" + ModalText);\r\n\r\n            GetModalText.innerHTML(Response);\r\n            GetModalWindow.style.display = '';\r\n\r\n        }\r\n        else\r\n        {\r\n            console.log(\"Status: \" + status + \". Ajax response:\" + request.responseText);\r\n        }\r\n\r\n    };\r\n\r\n    Request.onerror = function ()\r\n    {\r\n        console.log(\"An error has occurred during the processing.\");\r\n    };\r\n\r\n    Request.send();\r\n\r\n}\r\n\n\n//# sourceURL=webpack:///./scripts/functions/modals.js?");

/***/ }),

/***/ "./scripts/site.js":
/*!*************************!*\
  !*** ./scripts/site.js ***!
  \*************************/
/*! no exports provided */
/***/ (function(module, __webpack_exports__, __webpack_require__) {

"use strict";
eval("__webpack_require__.r(__webpack_exports__);\n/* harmony import */ var _functions_helpers__WEBPACK_IMPORTED_MODULE_0__ = __webpack_require__(/*! ./functions/helpers */ \"./scripts/functions/helpers.js\");\n/* harmony import */ var _functions_modals__WEBPACK_IMPORTED_MODULE_1__ = __webpack_require__(/*! ./functions/modals */ \"./scripts/functions/modals.js\");\n/* harmony import */ var _functions_ajax__WEBPACK_IMPORTED_MODULE_2__ = __webpack_require__(/*! ./functions/ajax */ \"./scripts/functions/ajax.js\");\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\nDOMReady(Initialize());\r\n\r\n\r\nfunction DOMReady(Initialize)\r\n{\r\n\r\n    if (document.readyState != 'loading')\r\n    {\r\n        Initialize();\r\n    }\r\n    else\r\n    {\r\n        document.addEventListener('DOMContentLoaded', Initialize);\r\n    }\r\n\r\n}\r\n\r\n\r\nfunction Initialize()\r\n{\r\n\r\n}\r\n\n\n//# sourceURL=webpack:///./scripts/site.js?");

/***/ }),

/***/ "./styles/site.scss":
/*!**************************!*\
  !*** ./styles/site.scss ***!
  \**************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("\nvar content = __webpack_require__(/*! !../node_modules/css-loader/dist/cjs.js!../node_modules/sass-loader/dist/cjs.js!./site.scss */ \"./node_modules/css-loader/dist/cjs.js!./node_modules/sass-loader/dist/cjs.js!./styles/site.scss\");\n\nif(typeof content === 'string') content = [[module.i, content, '']];\n\nvar transform;\nvar insertInto;\n\n\n\nvar options = {\"hmr\":true}\n\noptions.transform = transform\noptions.insertInto = undefined;\n\nvar update = __webpack_require__(/*! ../node_modules/style-loader/lib/addStyles.js */ \"./node_modules/style-loader/lib/addStyles.js\")(content, options);\n\nif(content.locals) module.exports = content.locals;\n\nif(false) {}\n\n//# sourceURL=webpack:///./styles/site.scss?");

/***/ }),

/***/ 0:
/*!**************************************************!*\
  !*** multi ./styles/site.scss ./scripts/site.js ***!
  \**************************************************/
/*! no static exports found */
/***/ (function(module, exports, __webpack_require__) {

eval("__webpack_require__(/*! ./styles/site.scss */\"./styles/site.scss\");\nmodule.exports = __webpack_require__(/*! ./scripts/site.js */\"./scripts/site.js\");\n\n\n//# sourceURL=webpack:///multi_./styles/site.scss_./scripts/site.js?");

/***/ })

},[[0,"runtime","vendors"]]]);