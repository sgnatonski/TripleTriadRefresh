$.ajaxSetup({
    headers: { 'srconnectionid': sessionStorage.getItem('srconnectionid') }
});

window.onerror = function (error, url, line) {
    //controller.sendLog({ acc: 'error', data: 'ERR:' + error + ' URL:' + url + ' L:' + line });
    //throw error;
};

function getCookie(name) {
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

ko.bindingHandlers.drag = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var $element = $(element),
                        dragOptions = {
                            revert: 'invalid',
                            revertDuration: 250,
                            cancel: 'span.handle',
                            zIndex: 1000,
                            cursor: "pointer",
                            addClasses: false,
                            distance: 10,
                            start: function (e, ui) {
                                if (valueAccessor().value) {
                                    valueAccessor().value(viewModel);
                                }
                            }
                        };

        $element.draggable(dragOptions);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var $element = $(element),
        active = valueAccessor();

        if (!active) {
            $element.draggable('disable');
        }
        else {
            $element.draggable('enable');
        }
    }
};

ko.bindingHandlers.drop = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var $element = $(element),
        value = valueAccessor() || {},
        handler = ko.utils.unwrapObservable(value.onDropComplete),
        dropOptions = {
            greedy: true,
            tolerance: 'pointer',
            addClasses: false,
            over: function (event, ui) {
                if (!$(element).is('.playerField, .enemyField')) {
                    $(element).addClass("card-slot-over");
                }
            },
            out: function (event, ui) {
                $(element).removeClass("card-slot-over");
            },
            drop: function (e, ui) {
                $(element).removeClass("card-slot-over");
                var index = parseInt($(element).parent().attr('index')) * 3 + parseInt($(element).attr('index')) + 1;
                handler(index);
            }
        };
        $element.droppable(dropOptions);
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var $element = $(element);
    }
};

ko.bindingHandlers.loadingWhen = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var 
            $element = $(element),
            currentPosition = $element.css("position"),
            $loader = $("<div>").addClass("glass-panel loader").hide();

        //add the loader
        $element.append($loader);

        //center the loader
        $loader.css({
            position: "absolute",
            top: "50%",
            left: "50%",
            "margin-left": -($loader.width() / 2) + "px",
            "margin-top": -($loader.height() / 2) + "px"
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var isLoading = ko.utils.unwrapObservable(valueAccessor()),
            $element = $(element),
            $childrenToHide = $element.children(":not(div.loader)"),
            $loader = $element.find("div.loader");

        $loader.stop();
        $loader.clearQueue();

        if (isLoading) {
            $childrenToHide.fadeOut("fast").attr("disabled", "disabled");
            $loader.show();
        }
        else {
            $loader.fadeOut("fast");
            $childrenToHide.fadeIn("fast").removeAttr("disabled");
        }
    }
};

ko.bindingHandlers.fadeVisible = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        $(element).stop();
        $(element).clearQueue();
        $(element).fadeIn('fast');
    }
};

ko.bindingHandlers.pulse = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor());
        if (value) {
            //$(element).effect("pulsate", { times: 5 }, 2000);
        }
    }
};

ko.bindingHandlers.scrollbar = {
    init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        $(element).tinyscrollbar();
    }
};

ko.bindingHandlers.counter = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            currentVal = ko.utils.unwrapObservable(value.value),
            endVal = ko.utils.unwrapObservable(value.max),
            maxDuration = ko.utils.unwrapObservable(value.maxDuration),
            tick = 50, interval, duration, color,
            jqElement = $(element);

        jqElement.css('color', '#FFF');

        if (endVal == currentVal) {
            value.isDone(true);
            return;
        }
        else if (endVal < 0) {
            endVal = -endVal;
            color = '#F00';
        }
        else {
            color = '#0F0';
        }

        if (maxDuration < endVal * tick) {
            tick = maxDuration / (endVal || 1);
        }

        duration = endVal * tick;

        setTimeout(function () {
            jqElement.animate({ 'color': color }, duration);

            interval = setInterval(function () {
                if (currentVal >= endVal) {
                    clearInterval(interval);
                    value.isDone(true);
                }
                else {
                    currentVal++;
                    jqElement.text(currentVal);
                }
            }, tick);
        }, 500);
    }
};

$.extendSignalR = function () {
    // TODO: this should be part of signalR library
    if ($.signalR.hub.clearProxy) {
        return;
    }

    $.extend($.signalR.hub, {
        clearProxy: function () {
            for (var key in $.signalR) {
                if ($.signalR.hasOwnProperty(key)) {
                    var hub = $.signalR[key];

                    if (!(hub._ && hub._.hubName)) {
                        // Not a client hub
                        continue;
                    }

                    // Create and store the hub proxy
                    hub._.proxy = this.createProxy(hub._.hubName);

                    $(hub._.proxy).off();
                    // Loop through all members on the hub and find client hub functions to subscribe to
                    for (var memberKey in hub) {
                        if (memberKey !== '_' && hub.hasOwnProperty(memberKey) && $.inArray(memberKey, hub._.ignoreMembers) === -1) {
                            delete hub[memberKey];
                        }
                    }
                }
            }
        },
        recreateProxy: function () {
            function makeProxyCallback(hub, callback) {
                return function () {
                    updateHubState(hub, this.state);

                    // Call the client hub method
                    callback.apply(hub, $.makeArray(arguments));
                };
            }

            function updateHubState(hub, newState) {
                var oldState = hub._.oldState;

                if (!oldState) {
                    // First time updating client state, just copy it all over
                    $.extend(hub, newState);
                    hub._.oldState = $.extend({}, newState);
                    return;
                }

                // Compare the old client state to current client state and preserve any changes
                for (var key in newState) {
                    if (typeof (oldState[key]) !== "undefined" && oldState[key] === newState[key]) {
                        // Key is different between old state and new state thus it's changed locally
                        continue;
                    }
                    hub[key] = newState[key];
                    hub._.oldState[key] = newState[key];
                }
            }

            var key, hub, memberKey, memberValue, proxy;

            for (key in $.signalR) {
                if ($.signalR.hasOwnProperty(key)) {
                    hub = $.signalR[key];

                    if (!(hub._ && hub._.hubName)) {
                        // Not a client hub
                        continue;
                    }

                    // Create and store the hub proxy
                    hub._.proxy = this.createProxy(hub._.hubName);

                    // Loop through all members on the hub and find client hub functions to subscribe to
                    for (memberKey in hub) {
                        if (hub.hasOwnProperty(memberKey)) {
                            memberValue = hub[memberKey];

                            if (memberKey === "_" || $.type(memberValue) !== "function"
                            || $.inArray(memberKey, hub._.ignoreMembers) >= 0) {
                                // Not a client hub function
                                continue;
                            }

                            // Subscribe to the hub event for this method
                            hub._.proxy.on(memberKey, makeProxyCallback(hub, memberValue));
                        }
                    }
                }
            }
        }
    });
}