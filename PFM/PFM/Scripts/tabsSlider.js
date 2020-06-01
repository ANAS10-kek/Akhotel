
!function(t,e){"object"==typeof exports&&"object"==typeof module?module.exports=e():"function"==typeof define&&define.amd?define("TabsSlider",[],e):"object"==typeof exports?exports.TabsSlider=e():t.TabsSlider=e()}(window,function(){return function(t){var e={};function s(i){if(e[i])return e[i].exports;var n=e[i]={i:i,l:!1,exports:{}};return t[i].call(n.exports,n,n.exports,s),n.l=!0,n.exports}return s.m=t,s.c=e,s.d=function(t,e,i){s.o(t,e)||Object.defineProperty(t,e,{enumerable:!0,get:i})},s.r=function(t){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(t,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(t,"__esModule",{value:!0})},s.t=function(t,e){if(1&e&&(t=s(t)),8&e)return t;if(4&e&&"object"==typeof t&&t&&t.__esModule)return t;var i=Object.create(null);if(s.r(i),Object.defineProperty(i,"default",{enumerable:!0,value:t}),2&e&&"string"!=typeof t)for(var n in t)s.d(i,n,function(e){return t[e]}.bind(null,n));return i},s.n=function(t){var e=t&&t.__esModule?function(){return t.default}:function(){return t};return s.d(e,"a",e),e},s.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},s.p="",s(s.s=0)}([function(t,e,s){s(1),t.exports=s(2)},function(t,e,s){},function(t,e,s){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var i=Object.assign||function(t){for(var e=1;e<arguments.length;e++){var s=arguments[e];for(var i in s)Object.prototype.hasOwnProperty.call(s,i)&&(t[i]=s[i])}return t},n=function(){function t(t,e){for(var s=0;s<e.length;s++){var i=e[s];i.enumerable=i.enumerable||!1,i.configurable=!0,"value"in i&&(i.writable=!0),Object.defineProperty(t,i.key,i)}}return function(e,s,i){return s&&t(e.prototype,s),i&&t(e,i),e}}();s(3);var r=function(t){return t&&t.__esModule?t:{default:t}}(s(4));var o=function(){function t(e,s){if(function(t,e){if(!(t instanceof e))throw new TypeError("Cannot call a class as a function")}(this,t),"string"==typeof e&&(e=document.querySelector(e)),!(e instanceof HTMLElement))throw Error("Check the argument of the selector");this.tabs=e,this.tabs&&!this.tabs.activated&&(this.tabs.activated=!0,this.tabs.setAttribute("data-tabs-active",""),this.settings=i({animate:!0,slide:0,draggable:!0,underline:!0,heightAnimate:!0,duration:500,easing:"cubic-bezier(0.0, 0.0, 0.2, 1)"},s),this._init())}return n(t,[{key:"_init",value:function(){this.bar=this.tabs.querySelector(".tabs__bar"),this.content=this.tabs.querySelector(".tabs__content"),this.controls=Array.prototype.slice.call(this.bar.querySelectorAll(".tabs__controls")),this.sections=Array.prototype.slice.call(this.content.querySelectorAll(".tabs__section")),this.offset=0,this.currentId=this.settings.slide,this.slidesLen=this.sections.length,this.transformProperty=r.default.supportCSS3("transform"),this.transitionProperty=r.default.supportCSS3("transition"),this.has3d=r.default.support3d(),this._dimmensions(),this.settings.underline&&this._setSliderLine(),this.settings.draggable&&(this.dragX,this.dragY,this.delta,this.target,this.dragFlag=!1),this._addEvents(),this.show(this.currentId)}},{key:"destroy",value:function(){var t=this;this._removeEvents(),this.bar.removeChild(this.line),this.content.classList.remove("has-grab"),this.controls[this.currentId].classList.remove("is-active"),this.tabs.removeAttribute("data-tabs-active"),delete this.tabs.activated,Object.keys(this).forEach(function(e){delete t[e]})}},{key:"_addEvents",value:function(){if(this.handlerClick=this._selectTab.bind(this),this.handlerResize=this._responsive.bind(this),this._handlerTabFocus=this._handlerTabFocus.bind(this),this.bar.addEventListener("click",this.handlerClick),this.content.addEventListener("focus",this._handlerTabFocus,!0),window.addEventListener("resize",this.handlerResize),this.settings.draggable){this.handlerStart=this._start.bind(this),this.handlerMove=this._move.bind(this),this.handlerEnd=this._end.bind(this),this.handlerLeave=this._leave.bind(this);var t=this.dragEvent.event();this.content.addEventListener(t.start,this.handlerStart),this.content.addEventListener(t.move,this.handlerMove),this.content.addEventListener(t.end,this.handlerEnd),this.content.addEventListener(t.leave,this.handlerLeave)}}},{key:"_removeEvents",value:function(){if(this.bar.removeEventListener("click",this.handlerClick),this.content.removeEventListener("focus",this._handlerTabFocus,!0),window.removeEventListener("resize",this.handlerResize),this.settings.draggable){var t=this.dragEvent.event();this.content.removeEventListener(t.start,this.handlerStart),this.content.removeEventListener(t.move,this.handlerMove),this.content.removeEventListener(t.end,this.handlerEnd),this.content.removeEventListener(t.leave,this.handlerLeave)}}},{key:"_handlerTabFocus",value:function(t){var e=this,s=t.target.closest(".tabs__section");if(s){this.tabs.scrollLeft=0,this.content.scrollTop=0,setTimeout(function(){e.content.scrollTop=0,e.tabs.scrollLeft=0},0);var i=this.sections.indexOf(s);this.show(i)}}},{key:"_setSliderLine",value:function(){this.line=i(document.createElement("span"),{className:"tabs__line"}),this.bar.appendChild(this.line),this._moveSliderLine(),this.settings.animate&&(this.line.style[this.transitionProperty]="\n        "+this.transformProperty+" "+this.settings.duration+"ms "+this.settings.easing+"\n      ")}},{key:"_moveSliderLine",value:function(){var t=this.controls[this.currentId],e=t.offsetWidth,s=t.offsetLeft,i=this.has3d?"translate3d("+s+"px, 0, 0)":"translateX("+s+"px)";this.line.style.transform=i+" scaleX("+e/this.w+")"}},{key:"_dimmensions",value:function(){var t=this;this.w=this.tabs.offsetWidth;var e=this.sections[this.currentId].offsetHeight;this.sections.forEach(function(e){e.style.width=t.w+"px"}),this.content.style.width=this.w*this.sections.length+"px",this.content.style.height=e+"px"}},{key:"_responsive",value:function(){this._dimmensions(),this.offset=-this.w*this.currentId,this._moveSlide(this.offset,!1)}},{key:"_selectTab",value:function(t){var e=t.target.closest(".tabs__controls");if(e){t.preventDefault();var s=this.controls.indexOf(e);s!==this.currentId&&this.show(s)}}},{key:"_moveSlide",value:function(t){var e=!(arguments.length>1&&void 0!==arguments[1])||arguments[1];if(this.settings.animate){var s=e?this.settings.duration:0,i=[this.transformProperty+" "+s+"ms "+this.settings.easing];this.settings.heightAnimate&&i.push("height "+s+"ms "+this.settings.easing),this.content.style[this.transitionProperty]=i.join(",")}this.has3d?this.content.style[this.transformProperty]="translate3d("+t+"px, 0, 0)":this.content.style[this.transformProperty]="translateX("+t+"px)";var n=this.sections[this.currentId].offsetHeight;this.content.style.height=n+"px",this.settings.underline&&this._moveSliderLine()}},{key:"_start",value:function(t){if(!this.dragFlag){var e=void 0;t.targetTouches?(this.target=t.targetTouches[0].target,e=t.targetTouches[0]):e=t,this.delta=0,this.dragX=e.pageX||e.clientX,this.dragY=e.pageY||e.clientY,this.dragFlag=!0,this.content.classList.add("has-grab")}}},{key:"_move",value:function(t){if(this.dragFlag){var e=void 0;if(t.targetTouches){if(t.targetTouches.length>1||this.target!==t.targetTouches[0].target)return;e=t.targetTouches[0]}else t.preventDefault(),e=t;var s=e.pageX||e.clientX,i=e.pageY||e.clientY;if(Math.abs(this.dragX-s)>=Math.abs(this.dragY-i)){if(this.delta=(this.dragX-s)/2,!this.settings.animate)return;this._moveSlide(parseInt(this.offset,10)-this.delta,!1)}}}},{key:"_end",value:function(){if(this.dragFlag){var t=this.delta<0?this.currentId-1:this.currentId+1;if(Math.abs(this.delta)<50||t>this.slidesLen-1||t<0)return this.dragFlag=!1,void this._moveSlide(this.offset);this.dragFlag=!1,this.target=null,this.show(t),this.content.classList.remove("has-grab")}}},{key:"_leave",value:function(){this.dragFlag&&(this._moveSlide(this.offset),this.dragFlag=!1,this.content.classList.remove("has-grab"))}},{key:"recalcStyles",value:function(){this._responsive()}},{key:"show",value:function(t){(t=Math.abs(t))>=this.slidesLen&&(t=this.slidesLen-1),this.controls[this.currentId].classList.remove("is-active");var e=this.currentId;this.currentId=t,this.offset=-this.w*this.currentId,this._moveSlide(this.offset),this.controls[this.currentId].classList.add("is-active");var s=document.createEvent("CustomEvent");s.initCustomEvent("tabChange",!0,!0,{currentIndex:this.currentId,prevIndex:e,currentSlide:this.sections[this.currentId],currentTab:this.controls[this.currentId]}),this.tabs.dispatchEvent(s)}},{key:"dragEvent",get:function(){return{hasTouch:!!("ontouchstart"in window||window.DocumentTouch&&document instanceof DocumentTouch),event:function(){return{start:this.hasTouch?"touchstart":"mousedown",move:this.hasTouch?"touchmove":"mousemove",end:this.hasTouch?"touchend":"mouseup",leave:this.hasTouch?"touchleave":"mouseleave"}}}}}]),t}();e.default=o,t.exports=e.default},function(t,e,s){"use strict";Element.prototype.matches||(Element.prototype.matches=Element.prototype.msMatchesSelector||Element.prototype.webkitMatchesSelector),Element.prototype.closest||(Element.prototype.closest=function(t){var e=this;do{if(e.matches(t))return e;e=e.parentElement||e.parentNode}while(null!==e&&1===e.nodeType);return null})},function(t,e,s){"use strict";Object.defineProperty(e,"__esModule",{value:!0}),e.default={supportCSS3:function(t){var e=["-webkit-","-moz-","-ms-",""],s=document.documentElement;function i(t){return t.replace(/\-([a-z])/gi,function(t,e){return e.toUpperCase()})}for(var n=e.length-1;n>=0;n--){var r=void 0;if((r=~e[n].indexOf("ms")?"msTransform":i(e[n]+t))in s.style)return r}return!1},transitionEnd:function(){var t={transition:"transitionend",WebkitTransition:"webkitTransitionEnd",MozTransition:"mozTransitionEnd"},e=document.documentElement;for(var s in t)if(void 0!==e.style[s])return t[s];return!1},support3d:function(){if(!window.getComputedStyle)return!1;var t,e=document.createElement("div"),s=this.supportCSS3("transform");return document.body.insertBefore(e,null),e.style[s]="translate3d(1px,1px,1px)",t=getComputedStyle(e)[s],document.body.removeChild(e),void 0!==t&&t.length>0&&"none"!==t}},t.exports=e.default}])});