; (function ($, document, undefined) {
    var pluses = /\+/g;
    function raw(s) {
        return s;
    }

    function decoded(s) {
        return decodeURIComponent(s.replace(pluses, ' '));
    }

    var config = $.cookie = function (key, value, options) {
        // write
        if (value !== undefined) {
            options = $.extend({}, config.defaults, options);

            if (value === null) {
                options.expires = -1;
            }

            if (typeof options.expires === 'number') {
                var days = options.expires, t = options.expires = new Date();
                t.setDate(t.getDate() + days);
            }

            value = config.json ? JSON.stringify(value) : String(value);

            return (document.cookie = [
                encodeURIComponent(key), '=', config.raw ? value : encodeURIComponent(value),
                options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
                options.path ? '; path=' + options.path : '',
                options.domain ? '; domain=' + options.domain : '',
                options.secure ? '; secure' : ''
            ].join(''));
        }
        // read
        var decode = config.raw ? raw : decoded;
        var cookies = document.cookie.split('; ');
        for (var i = 0, l = cookies.length; i < l; i++) {
            var parts = cookies[i].split('=');
            if (decode(parts.shift()) === key) {
                var cookie = decode(parts.join('='));
                return config.json ? JSON.parse(cookie) : cookie;
            }
        }
        return null;
    };
    config.defaults = {};
    $.removeCookie = function (key, options) {
        if ($.cookie(key) !== null) {
            $.cookie(key, null, options);
            return true;
        }
        return false;
    };
})(jQuery, document);

//; (function (e) { e.fn.countdown = function (t) { var n = { date: null, updateTime: 1e3, htmlTemplate: "%d <span class='cd-time'>days</span> %h <span class='cd-time'>hours</span> %i <span class='cd-time'>mins</span> %s <span class='cd-time'>sec</span>", minus: !1, onChange: null, onComplete: null, onResume: null, onPause: null, leadingZero: !1, offset: null, servertime: null, hoursOnly: !1, minsOnly: !1, secsOnly: !1, weeks: !1, hours: !1, yearsAndMonths: !1, direction: "down", stopwatch: !1 }, r = Array.prototype.slice, i = window.clearInterval, s = Math.floor, o = 36e5, u = 31556926, a = 2629743.83, f = 604800, l = 86400, c = 3600, h = 60, p = 1, d = /(%y|%m|%w|%d|%h|%i|%s)/g, v = /%y/, m = /%m/, g = /%w/, y = /%d/, b = /%h/, w = /%i/, E = /%s/, S = function (e) { var t = new Date, n = e.data("jcdData"); return n ? (n.offset !== null ? t = x(n.offset) : t = x(null, n.difference), t.setMilliseconds(0), t) : new Date }, x = function (e, t) { var n, r, i, s = new Date; return e === null ? r = s.getTime() - t : (n = e * o, i = s.getTime() - -s.getTimezoneOffset() / 60 * o + n, r = s.setTime(i)), new Date(r) }, T = function () { var e = this, t, n, r, o, x, T, N, C, k, L, A, O = "", M, _ = function (e) { var t; return t = s(M / e), M -= t * e, t }, D = e.data("jcdData"); if (!D) return !1; t = D.htmlTemplate, n = S(e), r = D.dateObj, r.setMilliseconds(0), o = D.direction === "down" ? r.getTime() - n.getTime() : n.getTime() - r.getTime(), M = Math.round(o / 1e3), C = _(l), k = _(c), L = _(h), A = _(p), D.yearsAndMonths && (M += C * l, x = _(u), T = _(a), C = _(l)), D.weeks && (M += C * l, N = _(f), C = _(l)), D.hoursOnly && (k += C * 24, C = 0), D.minsOnly && (L += k * 60 + C * 24 * 60, C = k = 0), D.secsOnly && (A += L * 60, C = k = L = 0), D.yearsLeft = x, D.monthsLeft = T, D.weeksLeft = N, D.daysLeft = C, D.hrsLeft = k, D.minsLeft = L, D.secLeft = A, A === 60 && (A = 0), D.leadingZero && (C < 10 && !D.hoursOnly && (C = "0" + C), x < 10 && (x = "0" + x), T < 10 && (T = "0" + T), N < 10 && (N = "0" + N), k < 10 && (k = "0" + k), L < 10 && (L = "0" + L), A < 10 && (A = "0" + A)), D.direction === "down" && (n < r || D.minus) || D.direction === "up" && (r < n || D.minus) ? (O = t.replace(v, x).replace(m, T).replace(g, N), O = O.replace(y, C).replace(b, k).replace(w, L).replace(E, A)) : (O = t.replace(d, "00"), D.hasCompleted = !0), e.html(O).trigger("change.jcdevt", [D]).trigger("countChange", [D]), D.hasCompleted && (e.trigger("complete.jcdevt").trigger("countComplete"), i(D.timer)), e.data("jcdData", D) }, N = { init: function (t) { var r = e.extend({}, n, t), i, s; return this.each(function () { var n = e(this), o = {}, u; n.data("jcdData") && (n.countdown("changeSettings", t, !0), r = n.data("jcdData")); if (r.date === null) return e.error("No Date passed to jCountdown. date option is required."), !0; s = new Date(r.date), s.toString() === "Invalid Date" && e.error("Invalid Date passed to jCountdown: " + r.date), s = null, r.onChange && n.on("change.jcdevt", r.onChange), r.onComplete && n.on("complete.jcdevt", r.onComplete), r.onPause && n.on("pause.jcdevt", r.onPause), r.onResume && n.on("resume.jcdevt", r.onResume), o = e.extend({}, r), o.originalHTML = n.html(), o.dateObj = new Date(r.date), o.hasCompleted = !1, o.timer = 0, o.yearsLeft = o.monthsLeft = o.weeksLeft = o.daysLeft = o.hrsLeft = o.minsLeft = o.secLeft = 0, o.difference = null; if (r.servertime !== null) { var a; i = new Date, a = e.isFunction(o.servertime) ? o.servertime() : o.servertime, o.difference = i.getTime() - a, a = null } u = e.proxy(T, n), o.timer = setInterval(u, o.updateTime), n.data("jcdData", o), u() }) }, changeSettings: function (t, n) { return this.each(function () { var r = e(this), s, o, u = e.proxy(T, r); if (!r.data("jcdData")) return !0; s = e.extend({}, r.data("jcdData"), t), t.hasOwnProperty("date") && (o = new Date(t.date), o.toString() === "Invalid Date" && e.error("Invalid Date passed to jCountdown: " + t.date)), s.hasCompleted = !1, s.dateObj = new Date(t.date), i(s.timer), r.off(".jcdevt").data("jcdData", s), n || (s.onChange && r.on("change.jcdevt", s.onChange), s.onComplete && r.on("complete.jcdevt", s.onComplete), s.onPause && r.on("pause.jcdevt", s.onPause), s.onResume && r.on("resume.jcdevt", s.onResume), s.timer = setInterval(u, s.updateTime), r.data("jcdData", s), u()), s = null }) }, resume: function () { return this.each(function () { var t = e(this), n = t.data("jcdData"), r = e.proxy(T, t); if (!n) return !0; t.data("jcdData", n).trigger("resume.jcdevt", [n]).trigger("countResume", [n]); if (!n.hasCompleted) { n.timer = setInterval(r, n.updateTime); if (n.stopwatch && n.direction === "up") { var i = S(t).getTime() - n.pausedAt.getTime(), s = new Date; s.setTime(n.dateObj.getTime() + i), n.dateObj = s } r() } }) }, pause: function () { return this.each(function () { var t = e(this), n = t.data("jcdData"); if (!n) return !0; n.stopwatch && (n.pausedAt = S(t)), i(n.timer), t.data("jcdData", n).trigger("pause.jcdevt", [n]).trigger("countPause", [n]) }) }, complete: function () { return this.each(function () { var t = e(this), n = t.data("jcdData"); if (!n) return !0; i(n.timer), n.hasCompleted = !0, t.data("jcdData", n).trigger("complete.jcdevt").trigger("countComplete", [n]).off(".jcdevt") }) }, destroy: function () { return this.each(function () { var t = e(this), n = t.data("jcdData"); if (!n) return !0; i(n.timer), t.off(".jcdevt").removeData("jcdData").html(n.originalHTML) }) }, getSettings: function (t) { var n = e(this), r = n.data("jcdData"); return t && r ? r.hasOwnProperty(t) ? r[t] : undefined : r } }; if (N[t]) return N[t].apply(this, r.call(arguments, 1)); if (typeof t == "object" || !t) return N.init.apply(this, arguments); e.error("Method " + t + " does not exist in the jCountdown Plugin") } })(jQuery)


var NowTest, xClock, xCierre;

; (function ($) {
    $.fn.countdown = function (method /*, options*/) {

        var defaults = {
            date: null,
            updateTime: 1000,
            htmlTemplate: "%d <span class='cd-time'>days</span> %h <span class='cd-time'>hours</span> %i <span class='cd-time'>mins</span> %s <span class='cd-time'>sec</span>",
            minus: false,
            onChange: null,
            onComplete: null,
            onResume: null,
            onPause: null,
            leadingZero: false,
            offset: null,
            servertime: null,
            hoursOnly: true,
            minsOnly: false,
            secsOnly: false,
            weeks: false,
            hours: false,
            yearsAndMonths: false,
            direction: "down",
            stopwatch: false
        },
		slice = Array.prototype.slice,
		clear = window.clearInterval,
		floor = Math.floor,
		msPerHr = 3600000,
		secPerYear = 31556926,
		secPerMonth = 2629743.83,
		secPerWeek = 604800,
		secPerDay = 86400,
		secPerHr = 3600,
		secPerMin = 60,
		secPerSec = 1,
		rDate = /(%y|%m|%w|%d|%h|%i|%s)/g,
		rYears = /%y/,
		rMonths = /%m/,
		rWeeks = /%w/,
		rDays = /%d/,
		rHrs = /%h/,
		rMins = /%i/,
		rSecs = /%s/,

		_noUso_dateNow = function ($this) {
		    var now = new Date(),
				settings = $this.data("jcdData");

		    if (!settings) {
		        return new Date();
		    }

		    if (settings.offset !== null) {
		        now = getTZDate(settings.offset);
		    } else {
		        now = getTZDate(null, settings.difference); //Date now
		    }

		    now.setMilliseconds(0);
		    //console.log(now);
		    return now;
		},
        dateNow = function ($this) {
            return xClock;
        },
		getTZDate = function (offset, difference) {
		    var hrs,
				dateMS,
				curHrs,
				tmpDate = new Date();
		    if (offset === null) {
		        dateMS = tmpDate.getTime() - difference;
		    } else {
		        hrs = offset * msPerHr;
		        curHrs = tmpDate.getTime() - ((-tmpDate.getTimezoneOffset() / 60) * msPerHr) + hrs;
		        dateMS = tmpDate.setTime(curHrs);
		    }
		    return new Date(dateMS);
		},
		timerFunc = function () {
		    //Function runs at set interval updating countdown
		    var $this = this,
				template,
				now,
				date,
				timeLeft,
				yearsLeft,
				monthsLeft,
				weeksLeft,
		    //eDaysLeft,
				daysLeft,
		    //eHrsLeft,
				hrsLeft,
				minsLeft,
		    //eMinsleft,
				secLeft,
				time = "",
				diff,
				extractSection = function (numSecs) {
				    var amount;

				    amount = floor(diff / numSecs);
				    diff -= amount * numSecs;

				    return amount;
				},
				settings = $this.data("jcdData");

		    if (!settings) {
		        return false;
		    }

		    template = settings.htmlTemplate;

		    now = dateNow($this);

		    date = settings.dateObj; //Date to countdown to

		    date.setMilliseconds(0);

		    timeLeft = (settings.direction === "down") ? date.getTime() - now.getTime() : now.getTime() - date.getTime();

		    diff = Math.round(timeLeft / 1000);

		    daysLeft = extractSection(secPerDay);
		    hrsLeft = extractSection(secPerHr);
		    minsLeft = extractSection(secPerMin);
		    secLeft = extractSection(secPerSec);

		    if (settings.yearsAndMonths) {

		        //Add days back on so we can calculate years easier
		        diff += (daysLeft * secPerDay);

		        yearsLeft = extractSection(secPerYear);
		        monthsLeft = extractSection(secPerMonth);
		        daysLeft = extractSection(secPerDay);
		    }

		    if (settings.weeks) {
		        //Add days back on so we can calculate weeks easier				
		        diff += (daysLeft * secPerDay);

		        weeksLeft = extractSection(secPerWeek);
		        daysLeft = extractSection(secPerDay);
		    }

		    //Assumes you are using dates within a month 
		    //as years and months aren't taken into account
		    if (settings.hoursOnly) {
		        hrsLeft += daysLeft * 24;
		        daysLeft = 0;
		    }

		    //Assumes you are only using dates in the near future 
		    //as years and months aren't taken into account
		    if (settings.minsOnly) {
		        minsLeft += (hrsLeft * 60) + ((daysLeft * 24) * 60);
		        daysLeft = hrsLeft = 0;
		    }

		    //Assumes you are only using dates in the near future 
		    //as years, months and days aren't taken into account
		    if (settings.secsOnly) {
		        secLeft += (minsLeft * 60);
		        daysLeft = hrsLeft = minsLeft = 0;
		    }

		    settings.yearsLeft = yearsLeft;
		    settings.monthsLeft = monthsLeft;
		    settings.weeksLeft = weeksLeft;
		    settings.daysLeft = daysLeft;
		    settings.hrsLeft = hrsLeft;
		    settings.minsLeft = minsLeft;
		    settings.secLeft = secLeft;

		    if (secLeft === 60) {
		        secLeft = 0;
		    }

		    //hrsLeft = floor(24 * e_daysLeft);

		    if (settings.leadingZero) {

		        if (daysLeft < 10 && !settings.hoursOnly) {
		            daysLeft = "0" + daysLeft;
		        }

		        if (yearsLeft < 10) {
		            yearsLeft = "0" + yearsLeft;
		        }

		        if (monthsLeft < 10) {
		            monthsLeft = "0" + monthsLeft;
		        }

		        if (weeksLeft < 10) {
		            weeksLeft = "0" + weeksLeft;
		        }

		        if (hrsLeft < 10) {
		            hrsLeft = "0" + hrsLeft;
		        }
		        if (minsLeft < 10) {
		            minsLeft = "0" + minsLeft;
		        }
		        if (secLeft < 10) {
		            secLeft = "0" + secLeft;
		        }
		    }

		    if ((settings.direction === "down" && (now < date || settings.minus)) || (settings.direction === "up" && (date < now || settings.minus))) {

		        time = template.replace(rYears, yearsLeft).replace(rMonths, monthsLeft).replace(rWeeks, weeksLeft);
		        time = time.replace(rDays, daysLeft).replace(rHrs, hrsLeft).replace(rMins, minsLeft).replace(rSecs, secLeft);

		    } else {
		        time = template.replace(rDate, "00");
		        settings.hasCompleted = true;
		    }

		    $this.html(time).trigger("change.jcdevt", [settings]).trigger("countChange", [settings]);

		    if (settings.hasCompleted) {
		        $this.trigger("complete.jcdevt").trigger("countComplete");
		        clear(settings.timer);
		    }

		    $this.data("jcdData", settings);
		},
		methods = {
		    init: function (options) {

		        var opts = $.extend({}, defaults, options),
					local,
					testDate;

		        return this.each(function () {
		            var $this = $(this),
						settings = {},
						func;

		            //If this element already has a countdown timer, just change the settings
		            if ($this.data("jcdData")) {
		                $this.countdown("changeSettings", options, true);
		                opts = $this.data("jcdData");
		            }

		            if (opts.date === null) {
		                $.error("No Date passed to jCountdown. date option is required.");
		                return true;
		            }

		            testDate = new Date(opts.date);

		            if (testDate.toString() === "Invalid Date") {
		                $.error("Invalid Date passed to jCountdown: " + opts.date);
		            }

		            testDate = null;

		            //Add event handlers where set
		            if (opts.onChange) {
		                $this.on("change.jcdevt", opts.onChange);
		            }

		            if (opts.onComplete) {
		                $this.on("complete.jcdevt", opts.onComplete);
		            }

		            if (opts.onPause) {
		                $this.on("pause.jcdevt", opts.onPause);
		            }

		            if (opts.onResume) {
		                $this.on("resume.jcdevt", opts.onResume);
		            }

		            settings = $.extend({}, opts);

		            settings.originalHTML = $this.html();
		            settings.dateObj = new Date(opts.date);
		            settings.hasCompleted = false;
		            settings.timer = 0;
		            settings.yearsLeft = settings.monthsLeft = settings.weeksLeft = settings.daysLeft = settings.hrsLeft = settings.minsLeft = settings.secLeft = 0;
		            settings.difference = null;

		            if (opts.servertime !== null) {
		                var tempTime;
		                local = new Date();

		                tempTime = ($.isFunction(settings.servertime)) ? settings.servertime() : settings.servertime;
		                settings.difference = local.getTime() - tempTime;

		                tempTime = null;
		            }

		            func = $.proxy(timerFunc, $this);
		            settings.timer = setInterval(func, settings.updateTime);

		            $this.data("jcdData", settings);

		            func();
		        });
		    },
		    changeSettings: function (options, internal) {
		        //Like resume but with resetting/changing options				
		        return this.each(function () {
		            var $this = $(this),
						settings,
						testDate,
						func = $.proxy(timerFunc, $this);

		            if (!$this.data("jcdData")) {
		                return true;
		            }

		            settings = $.extend({}, $this.data("jcdData"), options);

		            if (options.hasOwnProperty("date")) {
		                testDate = new Date(options.date);

		                if (testDate.toString() === "Invalid Date") {
		                    $.error("Invalid Date passed to jCountdown: " + options.date);
		                }
		            }

		            settings.hasCompleted = false;
		            settings.dateObj = new Date(options.date);

		            //Clear the timer, as it might not be needed
		            clear(settings.timer);
		            $this.off(".jcdevt").data("jcdData", settings);

		            //As this can be accessed via the init method as well,
		            //we need to check how this method is being accessed
		            if (!internal) {

		                if (settings.onChange) {
		                    $this.on("change.jcdevt", settings.onChange);
		                }

		                if (settings.onComplete) {
		                    $this.on("complete.jcdevt", settings.onComplete);
		                }

		                if (settings.onPause) {
		                    $this.on("pause.jcdevt", settings.onPause);
		                }

		                if (settings.onResume) {
		                    $this.on("resume.jcdevt", settings.onResume);
		                }

		                settings.timer = setInterval(func, settings.updateTime);
		                $this.data("jcdData", settings);
		                func(); //Needs to run straight away when changing settings
		            }

		            settings = null;
		        });
		    },
		    resume: function () {
		        //Resumes a countdown timer
		        return this.each(function () {
		            var $this = $(this),
						settings = $this.data("jcdData"),
						func = $.proxy(timerFunc, $this);

		            if (!settings) {
		                return true;
		            }

		            $this.data("jcdData", settings).trigger("resume.jcdevt", [settings]).trigger("countResume", [settings]);
		            //We only want to resume a countdown that hasn't finished
		            if (!settings.hasCompleted) {
		                settings.timer = setInterval(func, settings.updateTime);

		                if (settings.stopwatch && settings.direction === "up") {

		                    var t = dateNow($this).getTime() - settings.pausedAt.getTime(),
								d = new Date();
		                    d.setTime(settings.dateObj.getTime() + t);

		                    settings.dateObj = d; //This is internal date
		                }

		                func();
		            }
		        });
		    },
		    pause: function () {
		        //Pause a countdown timer			
		        return this.each(function () {
		            var $this = $(this),
						settings = $this.data("jcdData");

		            if (!settings) {
		                return true;
		            }

		            if (settings.stopwatch) {
		                settings.pausedAt = dateNow($this);
		            }
		            //Clear interval (Will be started on resume)
		            clear(settings.timer);
		            //Trigger pause event handler
		            $this.data("jcdData", settings).trigger("pause.jcdevt", [settings]).trigger("countPause", [settings]);
		        });
		    },
		    complete: function () {
		        return this.each(function () {
		            var $this = $(this),
						settings = $this.data("jcdData");

		            if (!settings) {
		                return true;
		            }
		            //Clear timer
		            clear(settings.timer);
		            settings.hasCompleted = true;
		            //Update setting, trigger complete event handler, then unbind all events
		            //We don"t delete the settings in case they need to be checked later on
		            $this.data("jcdData", settings).trigger("complete.jcdevt").trigger("countComplete", [settings]).off(".jcdevt");
		        });
		    },
		    destroy: function () {
		        return this.each(function () {
		            var $this = $(this),
						settings = $this.data("jcdData");

		            if (!settings) {
		                return true;
		            }
		            //Clear timer
		            clear(settings.timer);
		            //Unbind all events, remove data and put DOM Element back to its original state (HTML wise)
		            $this.off(".jcdevt").removeData("jcdData").html(settings.originalHTML);
		        });
		    },
		    getSettings: function (name) {
		        var $this = $(this),
					settings = $this.data("jcdData");

		        //If an individual setting is required
		        if (name && settings) {
		            //If it exists, return it
		            if (settings.hasOwnProperty(name)) {
		                return settings[name];
		            }
		            return undefined;
		        }
		        //Return all settings or undefined
		        return settings;
		    }
		};

        if (methods[method]) {
            return methods[method].apply(this, slice.call(arguments, 1));
        } else if (typeof method === "object" || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error("Method " + method + " does not exist in the jCountdown Plugin");
        }
    };

})(jQuery);

/**
 * Copyright (c) 2007-2012 Ariel Flesler - aflesler(at)gmail(dot)com | http://flesler.blogspot.com
 * Dual licensed under MIT and GPL.
 * @author Ariel Flesler
 * @version 1.4.3.1
 */
;(function($){var h=$.scrollTo=function(a,b,c){$(window).scrollTo(a,b,c)};h.defaults={axis:'xy',duration:parseFloat($.fn.jquery)>=1.3?0:1,limit:true};h.window=function(a){return $(window)._scrollable()};$.fn._scrollable=function(){return this.map(function(){var a=this,isWin=!a.nodeName||$.inArray(a.nodeName.toLowerCase(),['iframe','#document','html','body'])!=-1;if(!isWin)return a;var b=(a.contentWindow||a).document||a.ownerDocument||a;return/webkit/i.test(navigator.userAgent)||b.compatMode=='BackCompat'?b.body:b.documentElement})};$.fn.scrollTo=function(e,f,g){if(typeof f=='object'){g=f;f=0}if(typeof g=='function')g={onAfter:g};if(e=='max')e=9e9;g=$.extend({},h.defaults,g);f=f||g.duration;g.queue=g.queue&&g.axis.length>1;if(g.queue)f/=2;g.offset=both(g.offset);g.over=both(g.over);return this._scrollable().each(function(){if(e==null)return;var d=this,$elem=$(d),targ=e,toff,attr={},win=$elem.is('html,body');switch(typeof targ){case'number':case'string':if(/^([+-]=)?\d+(\.\d+)?(px|%)?$/.test(targ)){targ=both(targ);break}targ=$(targ,this);if(!targ.length)return;case'object':if(targ.is||targ.style)toff=(targ=$(targ)).offset()}$.each(g.axis.split(''),function(i,a){var b=a=='x'?'Left':'Top',pos=b.toLowerCase(),key='scroll'+b,old=d[key],max=h.max(d,a);if(toff){attr[key]=toff[pos]+(win?0:old-$elem.offset()[pos]);if(g.margin){attr[key]-=parseInt(targ.css('margin'+b))||0;attr[key]-=parseInt(targ.css('border'+b+'Width'))||0}attr[key]+=g.offset[pos]||0;if(g.over[pos])attr[key]+=targ[a=='x'?'width':'height']()*g.over[pos]}else{var c=targ[pos];attr[key]=c.slice&&c.slice(-1)=='%'?parseFloat(c)/100*max:c}if(g.limit&&/^\d+$/.test(attr[key]))attr[key]=attr[key]<=0?0:Math.min(attr[key],max);if(!i&&g.queue){if(old!=attr[key])animate(g.onAfterFirst);delete attr[key]}});animate(g.onAfter);function animate(a){$elem.animate(attr,f,g.easing,a&&function(){a.call(this,e,g)})}}).end()};h.max=function(a,b){var c=b=='x'?'Width':'Height',scroll='scroll'+c;if(!$(a).is('html,body'))return a[scroll]-$(a)[c.toLowerCase()]();var d='client'+c,html=a.ownerDocument.documentElement,body=a.ownerDocument.body;return Math.max(html[scroll],body[scroll])-Math.min(html[d],body[d])};function both(a){return typeof a=='object'?a:{top:a,left:a}}})(jQuery);




/**
* jQuery customfileinput plugin
* Author: Scott Jehl, scott@filamentgroup.com
* Modifications: Terry Young (terryyounghk at gmail dot com)
*
* ##1: Added 'option' parameter. Initially for optionally specifying a width. Default is 'inherit' (i.e. not specified)
* ##2: Added options 'buttonText', 'inputText' and 'changeText'
* ##3: Added option 'maxFileSize'. For this technique, @see: http://www.tizag.com/htmlT/htmlupload.php
* ##4: Added auto-truncating the file name if text exceeds the available width (Uses the window.onresize event)
* ##5: Added option 'showInputText'. Default is true
* ##6: Added event handler 'onChange'
*/
$.fn.customFileInput = function (options) {

    // ##1 ++
    var defaults = {
        width: 'inherit',
        buttonText: 'Seleccionar',
        changeText: 'Cambiar',
        inputText: 'Selección de Archivo',
        showInputText: true,
        maxFileSize: 0, // ##3 ++

        onChange: $.noop
    };

    // ##1 ++
    var opts = $.extend(true, {}, defaults, options);

    //apply events and styles for file input element
    var fileInput = $(this)
        .addClass('customfile-input') //add class for CSS
        .mouseover(function () { upload.addClass('customfile-hover'); })
        .mouseout(function () { upload.removeClass('customfile-hover'); })
        .focus(function () {
            upload.addClass('customfile-focus');
            fileInput.data('val', fileInput.val());
        })
        .blur(function () {
            upload.removeClass('customfile-focus');
            $(this).trigger('checkChange');
        })
         .bind('disable', function () {
             fileInput.attr('disabled', true);
             upload.addClass('customfile-disabled');
         })
        .bind('enable', function () {
            fileInput.removeAttr('disabled');
            upload.removeClass('customfile-disabled');
        })
        .bind('checkChange', function () {
            if (fileInput.val() && fileInput.val() != fileInput.data('val')) {
                fileInput.trigger('change');
            }
        })
        .bind('change', function () {
            // ##5 ++
            if (opts.showInputText) {

                //get file name
                var fileName = $(this).val().split(/\\/).pop();
                //console.log(fileName);

                $(this).data('text', fileName);

                //get file extension
                var fileExt = 'customfile-ext-' + fileName.split('.').pop().toLowerCase();

                //change text of button
                // uploadButton.text('Change'); // ##2 --
                uploadButton.text(opts.changeText); // ##2 ++

                //update the feedback
                uploadFeedback
                    .text(fileName) //set feedback text to filename
                    .removeClass(uploadFeedback.data('fileExt') || '') //remove any existing file extension class
                    .addClass(fileExt) //add file extension class
                    .data('fileExt', fileExt) //store file extension for class removal on next change
                    .addClass('customfile-feedback-populated'); //add class to show populated state


                autoTruncateFileName();
            }

            if ($.isFunction(opts.onChange)) {
                opts.onChange.apply(this, arguments);
            }
        })
        .click(function () { //for IE and Opera, make sure change fires after choosing a file, using an async callback
            fileInput.data('val', fileInput.val());
            setTimeout(function () {
                fileInput.trigger('checkChange');
            }, 100);
        });

    //create custom control container
    var upload = $('<div class="customfile"></div>');

    // ##1 ++
    upload.css({
        width: opts.width
    });

    //create custom control button
    // ##2
    var uploadButton = $('<span class="customfile-button" aria-hidden="true"></span>').html(opts.buttonText).appendTo(upload);
    //create custom control feedback
    // ##2
    var uploadFeedback = $('<span class="customfile-feedback" aria-hidden="true"></span>').html(opts.inputText).appendTo(upload);

    // ##3
    if (opts.maxFileSize > 0 && $('input[type="hidden"][name="MAX_FILE_SIZE"]').length == 0) {
        $('<input type="hidden" name="MAX_FILE_SIZE">').val(opts.maxFileSize).appendTo(upload);
    }


    // ##4 ++
    var autoTruncateFileName = function () {
        //get file name
        var fileName = fileInput.val() || opts.inputText.replace("&#243;", "ó");

        if (fileName.length) {
            var limit = 0, // ensuring we're not going into an infinite loop
                trimmedFileName = fileName;
            uploadFeedback
                .text(fileName)
                .css({ display: 'inline' });
            while (limit < 1024 && trimmedFileName.length > 0 && uploadButton.outerWidth() + uploadFeedback.outerWidth() + 5 >= uploadButton.parent().innerWidth()) {
                trimmedFileName = trimmedFileName.substr(0, trimmedFileName.length - 1);
                uploadFeedback.text(trimmedFileName + '...');
                limit++;
            }
            uploadFeedback.css({ display: 'block' }); // ##4
        }
    };

    //match disabled state
    if (fileInput.is('[disabled]')) {
        fileInput.trigger('disable');
    }

    uploadFeedback.data('text', opts.inputText);

    // ##5 ++
    if (!opts.showInputText) {
        uploadFeedback.hide();
        uploadButton
            .css({
                float: 'inherit',
                display: 'block' // take up the full width of the parent container
            })
            .parent()
            .css({
                padding: 0
            });
    } else {
        uploadFeedback.css({
            display: 'block'
        });

        $(window).bind('resize', autoTruncateFileName);

    }


    //on mousemove, keep file input under the cursor to steal click
    upload
        .mousemove(function (e) {
            fileInput.css({
                'left': e.pageX - upload.offset().left - fileInput.outerWidth() + 20, //position right side 20px right of cursor X)
                //'top': e.pageY - upload.offset().top - $(window).scrollTop() - 3
                'top': e.pageY - upload.offset().top - 20
            });
        })
        .insertAfter(fileInput); //insert after the input

    fileInput.appendTo(upload);

    //return jQuery
    return $(this);
}


/*
* jQuery timepicker addon
* By: Trent Richardson [http://trentrichardson.com]
* Version 0.7
*/
; (function ($) { function Timepicker(singleton) { if (typeof (singleton) === 'boolean' && singleton == true) { this.regional = []; this.regional[''] = { currentText: 'Ahora', ampm: false, timeFormat: 'hh:mm tt', timeOnlyTitle: 'Seleccione...', timeText: 'Horario', hourText: 'Hora', minuteText: 'Minuto', secondText: 'Segundo' }; this.defaults = { showButtonPanel: true, timeOnly: false, showHour: true, showMinute: true, showSecond: false, showTime: true, stepHour: 0.05, stepMinute: 0.05, stepSecond: 0.05, hour: 0, minute: 0, second: 0, hourMin: 0, minuteMin: 0, secondMin: 0, hourMax: 23, minuteMax: 59, secondMax: 59, hourGrid: 0, minuteGrid: 0, secondGrid: 0, alwaysSetTime: true }; $.extend(this.defaults, this.regional['']); } else { this.defaults = $.extend({}, $.timepicker.defaults); } }; Timepicker.prototype = { $input: null, $altInput: null, $timeObj: null, inst: null, hour_slider: null, minute_slider: null, second_slider: null, hour: 0, minute: 0, second: 0, ampm: '', formattedDate: '', formattedTime: '', formattedDateTime: '', addTimePicker: function (dp_inst) { var tp_inst = this; var currDT; if ((this.$altInput) && this.$altInput != null) { currDT = this.$input.val() + ' ' + this.$altInput.val(); } else { currDT = this.$input.val(); } var regstr = this.defaults.timeFormat.toString().replace(/h{1,2}/ig, '(\\d?\\d)').replace(/m{1,2}/ig, '(\\d?\\d)').replace(/s{1,2}/ig, '(\\d?\\d)').replace(/t{1,2}/ig, '(am|pm|a|p)?').replace(/\s/g, '\\s?') + '$'; if (!this.defaults.timeOnly) { var dp_dateFormat = $.datepicker._get(dp_inst, 'dateFormat'); regstr = '.{' + dp_dateFormat.length + ',}\\s+' + regstr; } var order = this.getFormatPositions(); var treg = currDT.match(new RegExp(regstr, 'i')); if (treg) { if (order.t !== -1) { this.ampm = ((treg[order.t] === undefined || treg[order.t].length === 0) ? '' : (treg[order.t].charAt(0).toUpperCase() == 'A') ? 'AM' : 'PM').toUpperCase(); } if (order.h !== -1) { if (this.ampm == 'AM' && treg[order.h] == '12') { this.hour = 0; } else if (this.ampm == 'PM' && treg[order.h] != '12') { this.hour = (parseFloat(treg[order.h]) + 12).toFixed(0); } else { this.hour = treg[order.h]; } } if (order.m !== -1) { this.minute = treg[order.m]; } if (order.s !== -1) { this.second = treg[order.s]; } } tp_inst.timeDefined = (treg) ? true : false; if (typeof (dp_inst.stay_open) !== 'boolean' || dp_inst.stay_open === false) { setTimeout(function () { tp_inst.injectTimePicker(dp_inst, tp_inst); }, 10); } else { tp_inst.injectTimePicker(dp_inst, tp_inst); } }, getFormatPositions: function () { var finds = this.defaults.timeFormat.toLowerCase().match(/(h{1,2}|m{1,2}|s{1,2}|t{1,2})/g); var orders = { h: -1, m: -1, s: -1, t: -1 }; if (finds) { for (var i = 0; i < finds.length; i++) { if (orders[finds[i].toString().charAt(0)] == -1) { orders[finds[i].toString().charAt(0)] = i + 1; } } } return orders; }, injectTimePicker: function (dp_inst, tp_inst) { var $dp = dp_inst.dpDiv; var opts = tp_inst.defaults; var hourMax = opts.hourMax - (opts.hourMax % opts.stepHour); var minMax = opts.minuteMax - (opts.minuteMax % opts.stepMinute); var secMax = opts.secondMax - (opts.secondMax % opts.stepSecond); if ($dp.find("div#ui-timepicker-div-" + dp_inst.id).length === 0) { var noDisplay = ' style="display:none;"'; var html = '<div class="ui-timepicker-div" id="ui-timepicker-div-' + dp_inst.id + '"><dl>' + '<dt class="ui_tpicker_time_label" id="ui_tpicker_time_label_' + dp_inst.id + '"' + ((opts.showTime) ? '' : noDisplay) + '>' + opts.timeText + '</dt>' + '<dd class="ui_tpicker_time" id="ui_tpicker_time_' + dp_inst.id + '"' + ((opts.showTime) ? '' : noDisplay) + '></dd>' + '<dt class="ui_tpicker_hour_label" id="ui_tpicker_hour_label_' + dp_inst.id + '"' + ((opts.showHour) ? '' : noDisplay) + '>' + opts.hourText + '</dt>'; if (opts.hourGrid > 0) { html += '<dd class="ui_tpicker_hour ui_tpicker_hour_' + opts.hourGrid + '">' + '<div id="ui_tpicker_hour_' + dp_inst.id + '"' + ((opts.showHour) ? '' : noDisplay) + '></div>' + '<div><table><tr>'; for (var h = 0; h < hourMax; h += opts.hourGrid) { var tmph = h; if (opts.ampm && h > 12) tmph = h - 12; else tmph = h; if (tmph < 10) tmph = '0' + tmph; if (opts.ampm) { if (h == 0) tmph = 12 + 'a'; else if (h < 12) tmph += 'a'; else tmph += 'p'; } html += '<td>' + tmph + '</td>'; } html += '</tr></table></div>' + '</dd>'; } else { html += '<dd class="ui_tpicker_hour" id="ui_tpicker_hour_' + dp_inst.id + '"' + ((opts.showHour) ? '' : noDisplay) + '></dd>'; } html += '<dt class="ui_tpicker_minute_label" id="ui_tpicker_minute_label_' + dp_inst.id + '"' + ((opts.showMinute) ? '' : noDisplay) + '>' + opts.minuteText + '</dt>'; if (opts.minuteGrid > 0) { html += '<dd class="ui_tpicker_minute ui_tpicker_minute_' + opts.minuteGrid + '">' + '<div id="ui_tpicker_minute_' + dp_inst.id + '"' + ((opts.showMinute) ? '' : noDisplay) + '></div>' + '<div><table><tr>'; for (var m = 0; m < minMax; m += opts.minuteGrid) { html += '<td>' + ((m < 10) ? '0' : '') + m + '</td>'; } html += '</tr></table></div>' + '</dd>'; } else { html += '<dd class="ui_tpicker_minute" id="ui_tpicker_minute_' + dp_inst.id + '"' + ((opts.showMinute) ? '' : noDisplay) + '></dd>' } html += '<dt class="ui_tpicker_second_label" id="ui_tpicker_second_label_' + dp_inst.id + '"' + ((opts.showSecond) ? '' : noDisplay) + '>' + opts.secondText + '</dt>'; if (opts.secondGrid > 0) { html += '<dd class="ui_tpicker_second ui_tpicker_second_' + opts.secondGrid + '">' + '<div id="ui_tpicker_second_' + dp_inst.id + '"' + ((opts.showSecond) ? '' : noDisplay) + '></div>' + '<table><table><tr>'; for (var s = 0; s < secMax; s += opts.secondGrid) { html += '<td>' + ((s < 10) ? '0' : '') + s + '</td>'; } html += '</tr></table></table>' + '</dd>'; } else { html += '<dd class="ui_tpicker_second" id="ui_tpicker_second_' + dp_inst.id + '"' + ((opts.showSecond) ? '' : noDisplay) + '></dd>'; } html += '</dl></div>'; $tp = $(html); if (opts.timeOnly === true) { $tp.prepend('<div class="ui-widget-header ui-helper-clearfix ui-corner-all">' + '<div class="ui-datepicker-title">' + opts.timeOnlyTitle + '</div>' + '</div>'); $dp.find('.ui-datepicker-header, .ui-datepicker-calendar').hide(); } tp_inst.hour_slider = $tp.find('#ui_tpicker_hour_' + dp_inst.id).slider({ orientation: "horizontal", value: tp_inst.hour, min: opts.hourMin, max: hourMax, step: opts.stepHour, slide: function (event, ui) { tp_inst.hour_slider.slider("option", "value", ui.value); tp_inst.onTimeChange(dp_inst, tp_inst); } }); tp_inst.minute_slider = $tp.find('#ui_tpicker_minute_' + dp_inst.id).slider({ orientation: "horizontal", value: tp_inst.minute, min: opts.minuteMin, max: minMax, step: opts.stepMinute, slide: function (event, ui) { tp_inst.minute_slider.slider("option", "value", ui.value); tp_inst.onTimeChange(dp_inst, tp_inst); } }); tp_inst.second_slider = $tp.find('#ui_tpicker_second_' + dp_inst.id).slider({ orientation: "horizontal", value: tp_inst.second, min: opts.secondMin, max: secMax, step: opts.stepSecond, slide: function (event, ui) { tp_inst.second_slider.slider("option", "value", ui.value); tp_inst.onTimeChange(dp_inst, tp_inst); } }); $tp.find(".ui_tpicker_hour td").each(function (index) { $(this).click(function () { var h = $(this).html(); if (opts.ampm) { var ap = h.substring(2).toLowerCase(); var aph = new Number(h.substring(0, 2)); if (ap == 'a') { if (aph == 12) h = 0; else h = aph; } else { if (aph == 12) h = 12; else h = aph + 12; } } tp_inst.hour_slider.slider("option", "value", h); tp_inst.onTimeChange(dp_inst, tp_inst); }); $(this).css({ 'cursor': "pointer", 'width': '1%', 'text-align': 'left' }); }); $tp.find(".ui_tpicker_minute td").each(function (index) { $(this).click(function () { tp_inst.minute_slider.slider("option", "value", $(this).html()); tp_inst.onTimeChange(dp_inst, tp_inst); }); $(this).css({ 'cursor': "pointer", 'width': '1%', 'text-align': 'left' }); }); $tp.find(".ui_tpicker_second td").each(function (index) { $(this).click(function () { tp_inst.second_slider.slider("option", "value", $(this).html()); tp_inst.onTimeChange(dp_inst, tp_inst); }); $(this).css({ 'cursor': "pointer", 'width': '1%', 'text-align': 'left' }); }); $dp.find('.ui-datepicker-calendar').after($tp); tp_inst.$timeObj = $('#ui_tpicker_time_' + dp_inst.id); if (dp_inst !== null) { var timeDefined = tp_inst.timeDefined; tp_inst.onTimeChange(dp_inst, tp_inst); tp_inst.timeDefined = timeDefined; } } }, onTimeChange: function (dp_inst, tp_inst) { var hour = tp_inst.hour_slider.slider('value'); var minute = tp_inst.minute_slider.slider('value'); var second = tp_inst.second_slider.slider('value'); var ampm = (hour < 11.5) ? 'AM' : 'PM'; hour = (hour >= 11.5 && hour < 12) ? 12 : hour; var hasChanged = false; if (tp_inst.hour != hour || tp_inst.minute != minute || tp_inst.second != second || (tp_inst.ampm.length > 0 && tp_inst.ampm != ampm)) { hasChanged = true; } tp_inst.hour = parseFloat(hour).toFixed(0); tp_inst.minute = parseFloat(minute).toFixed(0); tp_inst.second = parseFloat(second).toFixed(0); tp_inst.ampm = ampm; tp_inst.formatTime(tp_inst); tp_inst.$timeObj.text(tp_inst.formattedTime); if (hasChanged) { tp_inst.updateDateTime(dp_inst, tp_inst); tp_inst.timeDefined = true; } }, formatTime: function (tp_inst) { var tmptime = tp_inst.defaults.timeFormat.toString(); var hour12 = ((tp_inst.ampm == 'AM') ? (tp_inst.hour) : (tp_inst.hour % 12)); hour12 = (Number(hour12) === 0) ? 12 : hour12; if (tp_inst.defaults.ampm === true) { tmptime = tmptime.toString().replace(/hh/g, ((hour12 < 10) ? '0' : '') + hour12).replace(/h/g, hour12).replace(/mm/g, ((tp_inst.minute < 10) ? '0' : '') + tp_inst.minute).replace(/m/g, tp_inst.minute).replace(/ss/g, ((tp_inst.second < 10) ? '0' : '') + tp_inst.second).replace(/s/g, tp_inst.second).replace(/TT/g, tp_inst.ampm.toUpperCase()).replace(/tt/g, tp_inst.ampm.toLowerCase()).replace(/T/g, tp_inst.ampm.charAt(0).toUpperCase()).replace(/t/g, tp_inst.ampm.charAt(0).toLowerCase()); } else { tmptime = tmptime.toString().replace(/hh/g, ((tp_inst.hour < 10) ? '0' : '') + tp_inst.hour).replace(/h/g, tp_inst.hour).replace(/mm/g, ((tp_inst.minute < 10) ? '0' : '') + tp_inst.minute).replace(/m/g, tp_inst.minute).replace(/ss/g, ((tp_inst.second < 10) ? '0' : '') + tp_inst.second).replace(/s/g, tp_inst.second); tmptime = $.trim(tmptime.replace(/t/gi, '')); } tp_inst.formattedTime = tmptime; return tp_inst.formattedTime; }, updateDateTime: function (dp_inst, tp_inst) { var dt = new Date(dp_inst.selectedYear, dp_inst.selectedMonth, dp_inst.selectedDay); var dateFmt = $.datepicker._get(dp_inst, 'dateFormat'); var formatCfg = $.datepicker._getFormatConfig(dp_inst); this.formattedDate = $.datepicker.formatDate(dateFmt, (dt === null ? new Date() : dt), formatCfg); var formattedDateTime = this.formattedDate; var timeAvailable = dt !== null && tp_inst.timeDefined; if (this.defaults.timeOnly === true) { formattedDateTime = this.formattedTime; } else if (this.defaults.timeOnly !== true && (this.defaults.alwaysSetTime || timeAvailable)) { if ((this.$altInput) && this.$altInput != null) { this.$altInput.val(this.formattedTime); } else { formattedDateTime += ' ' + this.formattedTime; } } this.formattedDateTime = formattedDateTime; this.$input.val(formattedDateTime); this.$input.trigger("change"); }, setDefaults: function (settings) { extendRemove(this.defaults, settings || {}); return this; } }; jQuery.fn.datetimepicker = function (o) { var opts = (o === undefined ? {} : o); var input = $(this); var tp = new Timepicker(); var inlineSettings = {}; for (var attrName in tp.defaults) { var attrValue = input.attr('time:' + attrName); if (attrValue) { try { inlineSettings[attrName] = eval(attrValue); } catch (err) { inlineSettings[attrName] = attrValue; } } } tp.defaults = $.extend(tp.defaults, inlineSettings); var beforeShowFunc = function (input, inst) { tp.hour = tp.defaults.hour; tp.minute = tp.defaults.minute; tp.second = tp.defaults.second; tp.ampm = ''; tp.$input = $(input); if (opts.altField != undefined && opts.altField != '') tp.$altInput = $($.datepicker._get(inst, 'altField')); tp.inst = inst; tp.addTimePicker(inst); if ($.isFunction(opts.beforeShow)) { opts.beforeShow(input, inst); } }; var onChangeMonthYearFunc = function (year, month, inst) { tp.updateDateTime(inst, tp); if ($.isFunction(opts.onChangeMonthYear)) { opts.onChangeMonthYear(year, month, inst); } }; var onCloseFunc = function (dateText, inst) { if (tp.timeDefined === true && input.val() != '') { tp.updateDateTime(inst, tp); } if ($.isFunction(opts.onClose)) { opts.onClose(dateText, inst); } }; tp.defaults = $.extend({}, tp.defaults, opts, { beforeShow: beforeShowFunc, onChangeMonthYear: onChangeMonthYearFunc, onClose: onCloseFunc, timepicker: tp }); $(this).datepicker(tp.defaults); }; jQuery.fn.timepicker = function (opts) { opts = $.extend(opts, { timeOnly: true }); $(this).datetimepicker(opts); }; $.datepicker._base_selectDate = $.datepicker._selectDate; $.datepicker._selectDate = function (id, dateStr) { var target = $(id); var inst = this._getInst(target[0]); var tp_inst = $.datepicker._get(inst, 'timepicker'); if (tp_inst) { inst.inline = true; inst.stay_open = true; $.datepicker._base_selectDate(id, dateStr); inst.stay_open = false; inst.inline = false; this._notifyChange(inst); this._updateDatepicker(inst); } else { $.datepicker._base_selectDate(id, dateStr); } }; $.datepicker._base_updateDatepicker = $.datepicker._updateDatepicker; $.datepicker._updateDatepicker = function (inst) { if (typeof (inst.stay_open) !== 'boolean' || inst.stay_open === false) { this._base_updateDatepicker(inst); this._beforeShow(inst.input, inst); } }; $.datepicker._beforeShow = function (input, inst) { var beforeShow = this._get(inst, 'beforeShow'); if (beforeShow) { inst.stay_open = true; beforeShow.apply((inst.input ? inst.input[0] : null), [inst.input, inst]); inst.stay_open = false; } }; $.datepicker._base_doKeyPress = $.datepicker._doKeyPress; $.datepicker._doKeyPress = function (event) { var inst = $.datepicker._getInst(event.target); var tp_inst = $.datepicker._get(inst, 'timepicker'); if (tp_inst) { if ($.datepicker._get(inst, 'constrainInput')) { var dateChars = $.datepicker._possibleChars($.datepicker._get(inst, 'dateFormat')); var chr = String.fromCharCode(event.charCode === undefined ? event.keyCode : event.charCode); var chrl = chr.toLowerCase(); return event.ctrlKey || (chr < ' ' || !dateChars || dateChars.indexOf(chr) > -1 || event.keyCode == 58 || event.keyCode == 32 || chr == ':' || chr == ' ' || chrl == 'a' || chrl == 'p' || chrl == 'm'); } } else { return $.datepicker._base_doKeyPress(event); } }; $.datepicker._base_gotoToday = $.datepicker._gotoToday; $.datepicker._gotoToday = function (id) { $.datepicker._base_gotoToday(id); var target = $(id); var dp_inst = this._getInst(target[0]); var tp_inst = $.datepicker._get(dp_inst, 'timepicker'); if (tp_inst) { var date = new Date(); var hour = date.getHours(); var minute = date.getMinutes(); var second = date.getSeconds(); if ((hour < tp_inst.defaults.hourMin || hour > tp_inst.defaults.hourMax) || (minute < tp_inst.defaults.minuteMin || minute > tp_inst.defaults.minuteMax) || (second < tp_inst.defaults.secondMin || second > tp_inst.defaults.secondMax)) { hour = tp_inst.defaults.hourMin; minute = tp_inst.defaults.minuteMin; second = tp_inst.defaults.secondMin; } tp_inst.hour_slider.slider('value', hour); tp_inst.minute_slider.slider('value', minute); tp_inst.second_slider.slider('value', second); tp_inst.onTimeChange(dp_inst, tp_inst); } }; function extendRemove(target, props) { $.extend(target, props); for (var name in props) if (props[name] == null || props[name] == undefined) target[name] = props[name]; return target; }; $.timepicker = new Timepicker(true); })(jQuery);

/* Copyright (c) 2009 José Joaquín Núñez (josejnv@gmail.com) http://joaquinnunez.cl/blog/
* Licensed under GPL (http://www.opensource.org/licenses/gpl-2.0.php)
* Use only for non-commercial usage.
*
* Version : 0.5
*
* Requires: jQuery 1.2+
*/

(function ($) {
    jQuery.fn.Rut = function (options) {
        var defaults = {
            digito_verificador: null,
            on_error: function () { },
            on_success: function () { },
            validation: true,
            format: true,
            format_on: 'change'
        };

        var opts = $.extend(defaults, options);

        return this.each(function () {

            if (defaults.format) {
                jQuery(this).bind(defaults.format_on, function () {
                    jQuery(this).val(jQuery.Rut.formatear(jQuery(this).val(), defaults.digito_verificador == null));
                });
            }
            if (defaults.validation) {
                if (defaults.digito_verificador == null) {
                    jQuery(this).bind('blur', function () {
                        var rut = jQuery(this).val();
                        if (jQuery(this).val() != "" && !jQuery.Rut.validar(rut)) {
                            defaults.on_error();
                        }
                        else if (jQuery(this).val() != "") {
                            defaults.on_success();
                        }
                    });
                }
                else {
                    var id = jQuery(this).attr("id");
                    jQuery(defaults.digito_verificador).bind('blur', function () {
                        var rut = jQuery("#" + id).val() + "-" + jQuery(this).val();
                        if (jQuery(this).val() != "" && !jQuery.Rut.validar(rut)) {
                            defaults.on_error();
                        }
                        else if (jQuery(this).val() != "") {
                            defaults.on_success();
                        }
                    });
                }
            }
        });
    }
})(jQuery);

/**
Funciones
*/


jQuery.Rut = {

    formatear: function (Rut, digitoVerificador) {
        var sRut = new String(Rut);
        var sRutFormateado = '';
        sRut = jQuery.Rut.quitarFormato(sRut);
        if (digitoVerificador) {
            var sDV = sRut.charAt(sRut.length - 1);
            sRut = sRut.substring(0, sRut.length - 1);
        }
        while (sRut.length > 3) {
            sRutFormateado = "." + sRut.substr(sRut.length - 3) + sRutFormateado;
            sRut = sRut.substring(0, sRut.length - 3);
        }
        sRutFormateado = sRut + sRutFormateado;
        if (sRutFormateado != "" && digitoVerificador) {
            sRutFormateado += "-" + sDV;
        }
        else if (digitoVerificador) {
            sRutFormateado += sDV;
        }

        return sRutFormateado;
    },
    IsMayorMillon: function (rut) {

        var tmp = rut.split('-');
        var digv = tmp[1];
        var _rut = tmp[0];

        var sinformato = jQuery.Rut.quitarFormato(_rut);

        return (sinformato > 1000000 && sinformato < 99999999);
    
    },
    formatearKeyUp: function (rut) {

        //var tmp = rut.split('-');
        //var digv = tmp[1];
        //var _rut = tmp[0];


        //if (tmp.length > 1) {

        //    var sinformato = jQuery.Rut.quitarFormato(_rut);

        //    if (digv != "") {
        //        console.log("digv != ''");
        //        return $.Rut.formatearConDv(tmp);
        //    } else {
        //        console.log("test");
        //        return $.Rut.formatear(_rut) + "-" + digv;
        //    }
           
        //} else {
        //    return $.Rut.formatear(tmp);
        //}
    
    },
    formatearConDv: function (rut) {

        var tmp = rut.split('-');
        var digv = tmp[1];
        var _rut = tmp[0];

        //console.log(digv);

        var sinformato = jQuery.Rut.quitarFormato(_rut);

        var fullRut = $.Rut.formatear(sinformato) + "-" + digv;

        //console.log(fullRut);

        return fullRut;
    },

    quitarFormato: function (rut) {
        var strRut = new String(rut);
        while (strRut.indexOf(".") != -1) {
            strRut = strRut.replace(".", "");
        }
        while (strRut.indexOf("-") != -1) {
            strRut = strRut.replace("-", "");
        }

        return strRut;
    },

    digitoValido: function (dv) {
        if (dv != '0' && dv != '1' && dv != '2' && dv != '3' && dv != '4'
            && dv != '5' && dv != '6' && dv != '7' && dv != '8' && dv != '9'
            && dv != 'k' && dv != 'K') {
            return false;
        }
        return true;
    },

    digitoCorrecto: function (crut) {
        largo = crut.length;
        if (largo < 2) {
            return false;
        }
        if (largo > 2) {
            rut = crut.substring(0, largo - 1);
        }
        else {
            rut = crut.charAt(0);
        }
        dv = crut.charAt(largo - 1);
        jQuery.Rut.digitoValido(dv);

        if (rut == null || dv == null) {
            return 0;
        }

        dvr = jQuery.Rut.getDigito(rut);

        if (dvr != dv.toLowerCase()) {
            return false;
        }
        return true;
    },

    getDigito: function (rut) {
        var dvr = '0';
        suma = 0;
        mul = 2;
        for (i = rut.length - 1; i >= 0; i--) {
            suma = suma + rut.charAt(i) * mul;
            if (mul == 7) {
                mul = 2;
            }
            else {
                mul++;
            }
        }
        res = suma % 11;
        if (res == 1) {
            return 'k';
        }
        else if (res == 0) {
            return '0';
        }
        else {
            return 11 - res;
        }
    },

    validar: function (texto) {
        texto = jQuery.Rut.quitarFormato(texto);
        largo = texto.length;

        // rut muy corto
        if (largo < 2) {
            return false;
        }

        // verifica que los numeros correspondan a los de rut
        for (i = 0; i < largo; i++) {
            // numero o letra que no corresponda a los del rut
            if (!jQuery.Rut.digitoValido(texto.charAt(i))) {
                return false;
            }
        }

        var invertido = "";
        for (i = (largo - 1), j = 0; i >= 0; i--, j++) {
            invertido = invertido + texto.charAt(i);
        }
        var dtexto = "";
        dtexto = dtexto + invertido.charAt(0);
        dtexto = dtexto + '-';
        cnt = 0;

        for (i = 1, j = 2; i < largo; i++, j++) {
            if (cnt == 3) {
                dtexto = dtexto + '.';
                j++;
                dtexto = dtexto + invertido.charAt(i);
                cnt = 1;
            }
            else {
                dtexto = dtexto + invertido.charAt(i);
                cnt++;
            }
        }

        invertido = "";
        for (i = (dtexto.length - 1), j = 0; i >= 0; i--, j++) {
            invertido = invertido + dtexto.charAt(i);
        }

        if (jQuery.Rut.digitoCorrecto(texto)) {
            return true;
        }
        return false;
    }
};


/*
* jQuery Shorten plugin 1.0.0
*
* Copyright (c) 2013 Viral Patel
* http://viralpatel.net
*
* Licensed under the MIT license:
*   http://www.opensource.org/licenses/mit-license.php
*/

/*
** updated by Jeff Richardson
** Updated to use strict,
** IE 7 has a "bug" It is returning underfined when trying to reference string characters in this format
** content[i]. IE 7 allows content.charAt(i) This works fine in all modern browsers.
** I've also added brackets where they werent added just for readability (mostly for me).
*/

(function ($) {
    $.fn.shorten = function (settings) {

        "use strict";
        if ($(this).data('jquery.shorten')) {
            return false;
        }
        $(this).data('jquery.shorten', true);

        var config = {
            showChars: 50,
            ellipsesText: "...",
            moreText: "ver más",
            lessText: "ver menos",
            errMsg: null
        };

        if (settings) {
            $.extend(config, settings);
        }

        $(document).off("click", '.morelink');

        $(document).on({ click: function () {

            var $this = $(this);
            if ($this.hasClass('less')) {
                $this.removeClass('less');
                $this.html(config.moreText);
                $this.parent().prev().prev().show(); // shortcontent
                $this.parent().prev().hide(); // allcontent

            } else {
                $this.addClass('less');
                $this.html(config.lessText);
                $this.parent().prev().prev().hide(); // shortcontent
                $this.parent().prev().show(); // allcontent
            }
            return false;
        }
        }, '.morelink');

        return this.each(function () {
            var $this = $(this);

            var content = $this.html();
            var contentlen = $this.text().length;
            if (contentlen > config.showChars) {
                var c = content.substr(0, config.showChars);
                if (c.indexOf('<') >= 0) // If there's HTML don't want to cut it
                {
                    var inTag = false; // I'm in a tag?
                    var bag = ''; // Put the characters to be shown here
                    var countChars = 0; // Current bag size
                    var openTags = []; // Stack for opened tags, so I can close them later
                    var tagName = null;

                    for (var i = 0, r = 0; r <= config.showChars; i++) {
                        if (content[i] == '<' && !inTag) {
                            inTag = true;

                            // This could be "tag" or "/tag"
                            tagName = content.substring(i + 1, content.indexOf('>', i));

                            // If its a closing tag
                            if (tagName[0] == '/') {


                                if (tagName != '/' + openTags[0]) {
                                    config.errMsg = 'ERROR en HTML: the top of the stack should be the tag that closes';
                                } else {
                                    openTags.shift(); // Pops the last tag from the open tag stack (the tag is closed in the retult HTML!)
                                }

                            } else {
                                // There are some nasty tags that don't have a close tag like <br/>
                                if (tagName.toLowerCase() != 'br') {
                                    openTags.unshift(tagName); // Add to start the name of the tag that opens
                                }
                            }
                        }
                        if (inTag && content[i] == '>') {
                            inTag = false;
                        }

                        if (inTag) { bag += content.charAt(i); } // Add tag name chars to the result
                        else {
                            r++;
                            if (countChars <= config.showChars) {
                                bag += content.charAt(i); // Fix to ie 7 not allowing you to reference string characters using the []
                                countChars++;
                            } else // Now I have the characters needed
                            {
                                if (openTags.length > 0) // I have unclosed tags
                                {
                                    //console.log('They were open tags');
                                    //console.log(openTags);
                                    for (j = 0; j < openTags.length; j++) {
                                        //console.log('Cierro tag ' + openTags[j]);
                                        bag += '</' + openTags[j] + '>'; // Close all tags that were opened

                                        // You could shift the tag from the stack to check if you end with an empty stack, that means you have closed all open tags
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    c = bag;
                }

                var html = '<span class="shortcontent">' + c + '&nbsp;' + config.ellipsesText +
                '</span><span class="allcontent">' + content +
                '</span>&nbsp;&nbsp;<span><a href="javascript://nop/" class="morelink">' + config.moreText + '</a></span>';

                $this.html(html);
                $this.find(".allcontent").hide(); // Esconde el contenido completo para todos los textos
            }
        });

    };

})(jQuery);


; (function ($) {
    var TableSpanMethods = {
        horizontal: function (obj) {	//The horizontal merge method
            return this.each(function () {
                $this = $(this); //Using $this

                $.each($this.find("tr"), function (i, row) {	//Find all the rows
                    var previousTD = "X"; //Initial declaration
                    $.each($(row).find("td"), function (j, column) {	//Find all the columns
                        if (previousTD == "X") {	//If the previous is the init value, it assigns the column to the previous read item
                            previousTD = column;
                        } else {
                            if ($(column).hasClass("ignoreTD")) {	//If the column has the class ignoreTD, it skips it
                                previousTD = "X";
                            } else {
                                if ($.trim($(column).html().replace("&nbsp;", "")) == $.trim($(previousTD).html().replace("&nbsp;", "")) && $(previousTD).attr("rowspan") == $(column).attr("rowspan")) {	//We remove all &nbsp; characters
                                    $(column).remove(); //Remove the newer column
                                    var colcount = 0;

                                    if ($(previousTD).attr("colspan") == undefined) {
                                        colcount = 1;
                                    } else {
                                        colcount = parseInt($(previousTD).attr("colspan"));
                                    }

                                    $(previousTD).attr("colspan", colcount + 1)	//Increase colspan of existing column
                                } else {
                                    previousTD = column;
                                }
                            }
                        }
                    })
                })
            })
        },

        vertical: function (obj) {
            return this.each(function () {
                $this = $(this);
                var rowObject = new Object();
                var x = 0; var totalRows = 0;

                // Generate an object going cell by cell, taking in account colspan
                $.each($this.find("tr"), function (i, row) {	//Find all the rows
                    totalRows++;
                    x = 0; rowObject[i] = new Object();
                    $.each($(row).find("td"), function (j, column) {	//Find all the columns
                        rowObject[i][x] = column; //Store the column on the two-dimension array

                        if ($(column).attr("colspan") == undefined) {
                            x += 1;
                        } else {
                            for (y = x; y < x + parseInt($(column).attr("colspan")); y++) {
                                rowObject[i][y] = column;
                            }
                            x += parseInt($(column).attr("colspan"))
                        }
                    })
                })

                $.each(rowObject, function (a, b) {
                    if (a < totalRows - 1) {
                        for (y = 0; y < x; y++) {
                            if ($.trim($(rowObject[a][y]).text()) == $.trim($(rowObject[parseInt(a) + 1][y]).text()) && $(rowObject[a][y]).attr("colspan") == $(rowObject[parseInt(a) + 1][y]).attr("colspan")) {
                                if ($(rowObject[a][y]).hasClass("ignoreTD") || $(rowObject[parseInt(a) + 1][y]).hasClass("ignoreTD")) {
                                    var colcount = 0;
                                    if ($(rowObject[a][y]).attr("colspan") == undefined) {
                                        colcount = 1;
                                    } else {
                                        colcount = parseInt($(rowObject[a][y]).attr("colspan"));
                                    }
                                    y += colcount - 1;
                                } else {
                                    $(rowObject[parseInt(a) + 1][y]).remove();
                                    rowObject[parseInt(a) + 1][y] = rowObject[parseInt(a)][y]
                                    var rowcount = 0;

                                    if ($(rowObject[a][y]).attr("rowspan") == undefined) {
                                        rowcount = 1;
                                    } else {
                                        rowcount = parseInt($(rowObject[a][y]).attr("rowspan"));
                                    }

                                    rowcount++;
                                    $(rowObject[a][y]).attr("rowspan", rowcount)

                                    var colcount = 0;
                                    if ($(rowObject[a][y]).attr("colspan") == undefined) {
                                        colcount = 1;
                                    } else {
                                        colcount = parseInt($(rowObject[a][y]).attr("colspan"));
                                    }
                                    y += colcount - 1;
                                }
                            } else {
                                var colcount = 0;
                                if ($(rowObject[a][y]).attr("colspan") == undefined) {
                                    colcount = 1;
                                } else {
                                    colcount = parseInt($(rowObject[a][y]).attr("colspan"));
                                }
                                y += colcount - 1;
                            }
                        }
                    }
                });
            });
        }
    }

    $.fn.TableSpan = function (method) {
        method = method.toLowerCase();
        if (TableSpanMethods[method]) {
            return TableSpanMethods[method].apply(this);
        } else if (typeof method === 'object' || !method) {
            return methods.init.apply(this, arguments);
        } else {
            $.error('Method ' + method + ' does not exist on jQuery');
        }
    };

})(jQuery);


; (function (e) { e.fn.priceFormat = function (t) { var n = { prefix: "US$ ", suffix: "", centsSeparator: ".", thousandsSeparator: ",", limit: false, centsLimit: 2, clearPrefix: false, clearSufix: false, allowNegative: false, insertPlusSign: false, clearOnEmpty: false }; var t = e.extend(n, t); return this.each(function () { function v(e) { var t = ""; for (var n = 0; n < e.length; n++) { char_ = e.charAt(n); if (t.length == 0 && char_ == 0) char_ = false; if (char_ && char_.match(r)) { if (a) { if (t.length < a) t = t + char_ } else { t = t + char_ } } } return t } function m(e) { while (e.length < f + 1) e = "0" + e; return e } function g(t, n) { if (!n && (t === "" || t == g("0", true)) && d) return ""; var r = m(v(t)); var a = ""; var l = 0; if (f == 0) { o = ""; c = "" } var c = r.substr(r.length - f, f); var y = r.substr(0, r.length - f); r = f == 0 ? y : y + o + c; if (u || e.trim(u) != "") { for (var b = y.length; b > 0; b--) { char_ = y.substr(b - 1, 1); l++; if (l % 3 == 0) char_ = u + char_; a = char_ + a } if (a.substr(0, 1) == u) a = a.substring(1, a.length); r = f == 0 ? a : a + o + c } if (h && (y != 0 || c != 0)) { if (t.indexOf("-") != -1 && t.indexOf("+") < t.indexOf("-")) { r = "-" + r } else { if (!p) r = "" + r; else r = "+" + r } } if (i) r = i + r; if (s) r = r + s; return r } function y(e) { var t = e.keyCode ? e.keyCode : e.which; var r = String.fromCharCode(t); var i = false; var s = n.val(); var o = g(s + r); if (t >= 48 && t <= 57 || t >= 96 && t <= 105) i = true; if (t == 8) i = true; if (t == 9) i = true; if (t == 13) i = true; if (t == 46) i = true; if (t == 37) i = true; if (t == 39) i = true; if (h && (t == 189 || t == 109)) i = true; if (p && (t == 187 || t == 107)) i = true; if (!i) { e.preventDefault(); e.stopPropagation(); if (s != o) n.val(o) } } function b() { var e = n.val(); var t = g(e); if (e != t) n.val(t); if (parseFloat(e) == 0 && d) n.val("") } function w() { var e = n.val(); n.val(i + e) } function E() { var e = n.val(); n.val(e + s) } function S() { if (e.trim(i) != "" && l) { var t = n.val().split(i); n.val(t[1]) } } function x() { if (e.trim(s) != "" && c) { var t = n.val().split(s); n.val(t[0]) } } var n = e(this); var r = /[0-9]/; var i = t.prefix; var s = t.suffix; var o = t.centsSeparator; var u = t.thousandsSeparator; var a = t.limit; var f = t.centsLimit; var l = t.clearPrefix; var c = t.clearSuffix; var h = t.allowNegative; var p = t.insertPlusSign; var d = t.clearOnEmpty; if (p) h = true; e(this).bind("keydown.price_format", y); e(this).bind("keyup.price_format", b); e(this).bind("focusout.price_format", b); if (l) { e(this).bind("focusout.price_format", function () { S() }); e(this).bind("focusin.price_format", function () { w() }) } if (c) { e(this).bind("focusout.price_format", function () { x() }); e(this).bind("focusin.price_format", function () { E() }) } if (e(this).val().length > 0) { b(); S(); x() } }) }; e.fn.unpriceFormat = function () { return e(this).unbind(".price_format") }; e.fn.unmask = function () { var t = e(this).val(); var n = ""; for (var r in t) { if (!isNaN(t[r]) || t[r] == "-") n += t[r] } return n } })(jQuery)


/*! jQuery number 2.1.3 (c) github.com/teamdf/jquery-number | opensource.teamdf.com/license */
;(function(h){function r(f,a){if(this.createTextRange){var c=this.createTextRange();c.collapse(true);c.moveStart("character",f);c.moveEnd("character",a-f);c.select()}else if(this.setSelectionRange){this.focus();this.setSelectionRange(f,a)}}function s(f){var a=this.value.length;f=f.toLowerCase()=="start"?"Start":"End";if(document.selection){a=document.selection.createRange();var c;c=a.duplicate();c.expand("textedit");c.setEndPoint("EndToEnd",a);c=c.text.length-a.text.length;a=c+a.text.length;return f==
"Start"?c:a}else if(typeof this["selection"+f]!="undefined")a=this["selection"+f];return a}var q={codes:{188:44,109:45,190:46,191:47,192:96,220:92,222:39,221:93,219:91,173:45,187:61,186:59,189:45,110:46},shifts:{96:"~",49:"!",50:"@",51:"#",52:"$",53:"%",54:"^",55:"&",56:"*",57:"(",48:")",45:"_",61:"+",91:"{",93:"}",92:"|",59:":",39:'"',44:"<",46:">",47:"?"}};h.fn.number=function(f,a,c,k){k=typeof k==="undefined"?",":k;c=typeof c==="undefined"?".":c;a=typeof a==="undefined"?0:a;var j="\\u"+("0000"+
c.charCodeAt(0).toString(16)).slice(-4),o=RegExp("[^"+j+"0-9]","g"),p=RegExp(j,"g");if(f===true)return this.is("input:text")?this.on({"keydown.format":function(b){var d=h(this),e=d.data("numFormat"),g=b.keyCode?b.keyCode:b.which,m="",i=s.apply(this,["start"]),n=s.apply(this,["end"]),l="";l=false;if(q.codes.hasOwnProperty(g))g=q.codes[g];if(!b.shiftKey&&g>=65&&g<=90)g+=32;else if(!b.shiftKey&&g>=69&&g<=105)g-=48;else if(b.shiftKey&&q.shifts.hasOwnProperty(g))m=q.shifts[g];if(m=="")m=String.fromCharCode(g);
if(g!==8&&m!=c&&!m.match(/[0-9]/)){d=b.keyCode?b.keyCode:b.which;if(d==46||d==8||d==9||d==27||d==13||(d==65||d==82)&&(b.ctrlKey||b.metaKey)===true||(d==86||d==67)&&(b.ctrlKey||b.metaKey)===true||d>=35&&d<=39)return;b.preventDefault();return false}if(i==0&&n==this.value.length||d.val()==0)if(g===8){i=n=1;this.value="";e.init=a>0?-1:0;e.c=a>0?-(a+1):0;r.apply(this,[0,0])}else if(m===c){i=n=1;this.value="0"+c+Array(a+1).join("0");e.init=a>0?1:0;e.c=a>0?-(a+1):0}else{if(this.value.length===0){e.init=
a>0?-1:0;e.c=a>0?-a:0}}else e.c=n-this.value.length;if(a>0&&m==c&&i==this.value.length-a-1){e.c++;e.init=Math.max(0,e.init);b.preventDefault();l=this.value.length+e.c}else if(m==c){e.init=Math.max(0,e.init);b.preventDefault()}else if(a>0&&g==8&&i==this.value.length-a){b.preventDefault();e.c--;l=this.value.length+e.c}else if(a>0&&g==8&&i>this.value.length-a){if(this.value==="")return;if(this.value.slice(i-1,i)!="0"){l=this.value.slice(0,i-1)+"0"+this.value.slice(i);d.val(l.replace(o,"").replace(p,
c))}b.preventDefault();e.c--;l=this.value.length+e.c}else if(g==8&&this.value.slice(i-1,i)==k){b.preventDefault();e.c--;l=this.value.length+e.c}else if(a>0&&i==n&&this.value.length>a+1&&i>this.value.length-a-1&&isFinite(+m)&&!b.metaKey&&!b.ctrlKey&&!b.altKey&&m.length===1){this.value=l=n===this.value.length?this.value.slice(0,i-1):this.value.slice(0,i)+this.value.slice(i+1);l=i}l!==false&&r.apply(this,[l,l]);d.data("numFormat",e)},"keyup.format":function(b){var d=h(this),e=d.data("numFormat");b=b.keyCode?
b.keyCode:b.which;var g=s.apply(this,["start"]);if(!(this.value===""||(b<48||b>57)&&(b<96||b>105)&&b!==8)){d.val(d.val());if(a>0)if(e.init<1){g=this.value.length-a-(e.init<0?1:0);e.c=g-this.value.length;e.init=1;d.data("numFormat",e)}else if(g>this.value.length-a&&b!=8){e.c++;d.data("numFormat",e)}d=this.value.length+e.c;r.apply(this,[d,d])}},"paste.format":function(b){var d=h(this),e=b.originalEvent,g=null;if(window.clipboardData&&window.clipboardData.getData)g=window.clipboardData.getData("Text");
else if(e.clipboardData&&e.clipboardData.getData)g=e.clipboardData.getData("text/plain");d.val(g);b.preventDefault();return false}}).each(function(){var b=h(this).data("numFormat",{c:-(a+1),decimals:a,thousands_sep:k,dec_point:c,regex_dec_num:o,regex_dec:p,init:false});this.value!==""&&b.val(b.val())}):this.each(function(){var b=h(this),d=+b.text().replace(o,"").replace(p,".");b.number(!isFinite(d)?0:+d,a,c,k)});return this.text(h.number.apply(window,arguments))};var t=null,u=null;if(h.isPlainObject(h.valHooks.text)){if(h.isFunction(h.valHooks.text.get))t=
h.valHooks.text.get;if(h.isFunction(h.valHooks.text.set))u=h.valHooks.text.set}else h.valHooks.text={};h.valHooks.text.get=function(f){var a=h(f).data("numFormat");if(a){if(f.value==="")return"";f=+f.value.replace(a.regex_dec_num,"").replace(a.regex_dec,".");return""+(isFinite(f)?f:0)}else if(h.isFunction(t))return t(f)};h.valHooks.text.set=function(f,a){var c=h(f).data("numFormat");if(c)return f.value=h.number(a,c.decimals,c.dec_point,c.thousands_sep);else if(h.isFunction(u))return u(f,a)};h.number=
function(f,a,c,k){k=typeof k==="undefined"?",":k;c=typeof c==="undefined"?".":c;a=!isFinite(+a)?0:Math.abs(a);var j="\\u"+("0000"+c.charCodeAt(0).toString(16)).slice(-4),o="\\u"+("0000"+k.charCodeAt(0).toString(16)).slice(-4);f=(f+"").replace(".",c).replace(RegExp(o,"g"),"").replace(RegExp(j,"g"),".").replace(RegExp("[^0-9+-Ee.]","g"),"");f=!isFinite(+f)?0:+f;j="";j=function(p,b){var d=Math.pow(10,b);return""+Math.round(p*d)/d};j=(a?j(f,a):""+Math.round(f)).split(".");if(j[0].length>3)j[0]=j[0].replace(/\B(?=(?:\d{3})+(?!\d))/g,
k);if((j[1]||"").length<a){j[1]=j[1]||"";j[1]+=Array(a-j[1].length+1).join("0")}return j.join(c)}})(jQuery);


/**
* @license Input Mask plugin for jquery
* http://github.com/RobinHerbots/jquery.inputmask
* Copyright (c) 2010 - 2014 Robin Herbots
* Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
* Version: 2.5.0
*/

(function ($) {
    if ($.fn.inputmask === undefined) {
        //helper functions    
        function isInputEventSupported(eventName) {
            var el = document.createElement('input'),
            eventName = 'on' + eventName,
            isSupported = (eventName in el);
            if (!isSupported) {
                el.setAttribute(eventName, 'return;');
                isSupported = typeof el[eventName] == 'function';
            }
            el = null;
            return isSupported;
        }
        function resolveAlias(aliasStr, options, opts) {
            var aliasDefinition = opts.aliases[aliasStr];
            if (aliasDefinition) {
                if (aliasDefinition.alias) resolveAlias(aliasDefinition.alias, undefined, opts); //alias is another alias
                $.extend(true, opts, aliasDefinition);  //merge alias definition in the options
                $.extend(true, opts, options);  //reapply extra given options
                return true;
            }
            return false;
        }
        function generateMaskSets(opts) {
            var ms = [];
            var genmasks = []; //used to keep track of the masks that where processed, to avoid duplicates
            function getMaskTemplate(mask) {
                if (opts.numericInput) {
                    mask = mask.split('').reverse().join('');
                }
                var escaped = false, outCount = 0, greedy = opts.greedy, repeat = opts.repeat;
                if (repeat == "*") greedy = false;
                //if (greedy == true && opts.placeholder == "") opts.placeholder = " ";
                if (mask.length == 1 && greedy == false && repeat != 0) { opts.placeholder = ""; } //hide placeholder with single non-greedy mask
                var singleMask = $.map(mask.split(""), function (element, index) {
                    var outElem = [];
                    if (element == opts.escapeChar) {
                        escaped = true;
                    }
                    else if ((element != opts.optionalmarker.start && element != opts.optionalmarker.end) || escaped) {
                        var maskdef = opts.definitions[element];
                        if (maskdef && !escaped) {
                            for (var i = 0; i < maskdef.cardinality; i++) {
                                outElem.push(opts.placeholder.charAt((outCount + i) % opts.placeholder.length));
                            }
                        } else {
                            outElem.push(element);
                            escaped = false;
                        }
                        outCount += outElem.length;
                        return outElem;
                    }
                });

                //allocate repetitions
                var repeatedMask = singleMask.slice();
                for (var i = 1; i < repeat && greedy; i++) {
                    repeatedMask = repeatedMask.concat(singleMask.slice());
                }

                return { "mask": repeatedMask, "repeat": repeat, "greedy": greedy };
            }
            //test definition => {fn: RegExp/function, cardinality: int, optionality: bool, newBlockMarker: bool, offset: int, casing: null/upper/lower, def: definitionSymbol}
            function getTestingChain(mask) {
                if (opts.numericInput) {
                    mask = mask.split('').reverse().join('');
                }
                var isOptional = false, escaped = false;
                var newBlockMarker = false; //indicates wheter the begin/ending of a block should be indicated

                return $.map(mask.split(""), function (element, index) {
                    var outElem = [];

                    if (element == opts.escapeChar) {
                        escaped = true;
                    } else if (element == opts.optionalmarker.start && !escaped) {
                        isOptional = true;
                        newBlockMarker = true;
                    }
                    else if (element == opts.optionalmarker.end && !escaped) {
                        isOptional = false;
                        newBlockMarker = true;
                    }
                    else {
                        var maskdef = opts.definitions[element];
                        if (maskdef && !escaped) {
                            var prevalidators = maskdef["prevalidator"], prevalidatorsL = prevalidators ? prevalidators.length : 0;
                            for (var i = 1; i < maskdef.cardinality; i++) {
                                var prevalidator = prevalidatorsL >= i ? prevalidators[i - 1] : [], validator = prevalidator["validator"], cardinality = prevalidator["cardinality"];
                                outElem.push({ fn: validator ? typeof validator == 'string' ? new RegExp(validator) : new function () { this.test = validator; } : new RegExp("."), cardinality: cardinality ? cardinality : 1, optionality: isOptional, newBlockMarker: isOptional == true ? newBlockMarker : false, offset: 0, casing: maskdef["casing"], def: maskdef["definitionSymbol"] || element });
                                if (isOptional == true) //reset newBlockMarker
                                    newBlockMarker = false;
                            }
                            outElem.push({ fn: maskdef.validator ? typeof maskdef.validator == 'string' ? new RegExp(maskdef.validator) : new function () { this.test = maskdef.validator; } : new RegExp("."), cardinality: maskdef.cardinality, optionality: isOptional, newBlockMarker: newBlockMarker, offset: 0, casing: maskdef["casing"], def: maskdef["definitionSymbol"] || element });
                        } else {
                            outElem.push({ fn: null, cardinality: 0, optionality: isOptional, newBlockMarker: newBlockMarker, offset: 0, casing: null, def: element });
                            escaped = false;
                        }
                        //reset newBlockMarker
                        newBlockMarker = false;
                        return outElem;
                    }
                });
            }
            function markOptional(maskPart) { //needed for the clearOptionalTail functionality
                return opts.optionalmarker.start + maskPart + opts.optionalmarker.end;
            }
            function splitFirstOptionalEndPart(maskPart) {
                var optionalStartMarkers = 0, optionalEndMarkers = 0, mpl = maskPart.length;
                for (var i = 0; i < mpl; i++) {
                    if (maskPart.charAt(i) == opts.optionalmarker.start) {
                        optionalStartMarkers++;
                    }
                    if (maskPart.charAt(i) == opts.optionalmarker.end) {
                        optionalEndMarkers++;
                    }
                    if (optionalStartMarkers > 0 && optionalStartMarkers == optionalEndMarkers)
                        break;
                }
                var maskParts = [maskPart.substring(0, i)];
                if (i < mpl) {
                    maskParts.push(maskPart.substring(i + 1, mpl));
                }
                return maskParts;
            }
            function splitFirstOptionalStartPart(maskPart) {
                var mpl = maskPart.length;
                for (var i = 0; i < mpl; i++) {
                    if (maskPart.charAt(i) == opts.optionalmarker.start) {
                        break;
                    }
                }
                var maskParts = [maskPart.substring(0, i)];
                if (i < mpl) {
                    maskParts.push(maskPart.substring(i + 1, mpl));
                }
                return maskParts;
            }
            function generateMask(maskPrefix, maskPart, metadata) {
                var maskParts = splitFirstOptionalEndPart(maskPart);
                var newMask, maskTemplate;

                var masks = splitFirstOptionalStartPart(maskParts[0]);
                if (masks.length > 1) {
                    newMask = maskPrefix + masks[0] + markOptional(masks[1]) + (maskParts.length > 1 ? maskParts[1] : "");
                    if ($.inArray(newMask, genmasks) == -1 && newMask != "") {
                        genmasks.push(newMask);
                        maskTemplate = getMaskTemplate(newMask);
                        ms.push({
                            "mask": newMask,
                            "_buffer": maskTemplate["mask"],
                            "buffer": maskTemplate["mask"].slice(),
                            "tests": getTestingChain(newMask),
                            "lastValidPosition": -1,
                            "greedy": maskTemplate["greedy"],
                            "repeat": maskTemplate["repeat"],
                            "metadata": metadata
                        });
                    }
                    newMask = maskPrefix + masks[0] + (maskParts.length > 1 ? maskParts[1] : "");
                    if ($.inArray(newMask, genmasks) == -1 && newMask != "") {
                        genmasks.push(newMask);
                        maskTemplate = getMaskTemplate(newMask);
                        ms.push({
                            "mask": newMask,
                            "_buffer": maskTemplate["mask"],
                            "buffer": maskTemplate["mask"].slice(),
                            "tests": getTestingChain(newMask),
                            "lastValidPosition": -1,
                            "greedy": maskTemplate["greedy"],
                            "repeat": maskTemplate["repeat"],
                            "metadata": metadata
                        });
                    }
                    if (splitFirstOptionalStartPart(masks[1]).length > 1) { //optional contains another optional
                        generateMask(maskPrefix + masks[0], masks[1] + maskParts[1], metadata);
                    }
                    if (maskParts.length > 1 && splitFirstOptionalStartPart(maskParts[1]).length > 1) {
                        generateMask(maskPrefix + masks[0] + markOptional(masks[1]), maskParts[1], metadata);
                        generateMask(maskPrefix + masks[0], maskParts[1], metadata);
                    }
                }
                else {
                    newMask = maskPrefix + maskParts;
                    if ($.inArray(newMask, genmasks) == -1 && newMask != "") {
                        genmasks.push(newMask);
                        maskTemplate = getMaskTemplate(newMask);
                        ms.push({
                            "mask": newMask,
                            "_buffer": maskTemplate["mask"],
                            "buffer": maskTemplate["mask"].slice(),
                            "tests": getTestingChain(newMask),
                            "lastValidPosition": -1,
                            "greedy": maskTemplate["greedy"],
                            "repeat": maskTemplate["repeat"],
                            "metadata": metadata
                        });
                    }
                }

            }

            if ($.isFunction(opts.mask)) { //allow mask to be a preprocessing fn - should return a valid mask
                opts.mask = opts.mask.call(this, opts);
            }
            if ($.isArray(opts.mask)) {
                $.each(opts.mask, function (ndx, msk) {
                    if (msk["mask"] != undefined) {
                        generateMask("", msk["mask"].toString(), msk);
                    } else
                        generateMask("", msk.toString());
                });
            } else generateMask("", opts.mask.toString());

            return opts.greedy ? ms : ms.sort(function (a, b) { return a["mask"].length - b["mask"].length; });
        }

        var msie1x = typeof ScriptEngineMajorVersion === "function"
                        ? ScriptEngineMajorVersion() //IE11 detection
                        : new Function("/*@cc_on return @_jscript_version; @*/")() >= 10, //conditional compilation from mickeysoft trick
            iphone = navigator.userAgent.match(new RegExp("iphone", "i")) !== null,
            android = navigator.userAgent.match(new RegExp("android.*safari.*", "i")) !== null,
            androidchrome = navigator.userAgent.match(new RegExp("android.*chrome.*", "i")) !== null,
            androidfirefox = navigator.userAgent.match(new RegExp("android.*firefox.*", "i")) !== null,
            PasteEventType = isInputEventSupported('paste') ? 'paste' : isInputEventSupported('input') ? 'input' : "propertychange";

        //if (androidchrome) {
        //    var browser = navigator.userAgent.match(new RegExp("chrome.*", "i")),
        //        version = parseInt(new RegExp(/[0-9]+/).exec(browser));
        //    androidchrome32 = (version == 32);
        //}

        //masking scope
        //actionObj definition see below
        function maskScope(masksets, activeMasksetIndex, opts, actionObj) {
            var isRTL = false,
                valueOnFocus = getActiveBuffer().join(''),
                $el,
                skipKeyPressEvent = false, //Safari 5.1.x - modal dialog fires keypress twice workaround
                skipInputEvent = false, //skip when triggered from within inputmask
                ignorable = false;


            //maskset helperfunctions

            function getActiveMaskSet() {
                return masksets[activeMasksetIndex];
            }

            function getActiveTests() {
                return getActiveMaskSet()['tests'];
            }

            function getActiveBufferTemplate() {
                return getActiveMaskSet()['_buffer'];
            }

            function getActiveBuffer() {
                return getActiveMaskSet()['buffer'];
            }

            function isValid(pos, c, strict) { //strict true ~ no correction or autofill
                strict = strict === true; //always set a value to strict to prevent possible strange behavior in the extensions 

                function _isValid(position, activeMaskset, c, strict) {
                    var testPos = determineTestPosition(position), loopend = c ? 1 : 0, chrs = '', buffer = activeMaskset["buffer"];
                    for (var i = activeMaskset['tests'][testPos].cardinality; i > loopend; i--) {
                        chrs += getBufferElement(buffer, testPos - (i - 1));
                    }

                    if (c) {
                        chrs += c;
                    }

                    //return is false or a json object => { pos: ??, c: ??} or true
                    return activeMaskset['tests'][testPos].fn != null ?
                        activeMaskset['tests'][testPos].fn.test(chrs, buffer, position, strict, opts)
                        : (c == getBufferElement(activeMaskset['_buffer'].slice(), position, true) || c == opts.skipOptionalPartCharacter) ?
                            { "refresh": true, c: getBufferElement(activeMaskset['_buffer'].slice(), position, true), pos: position }
                            : false;
                }

                function PostProcessResults(maskForwards, results) {
                    var hasValidActual = false;
                    $.each(results, function (ndx, rslt) {
                        hasValidActual = $.inArray(rslt["activeMasksetIndex"], maskForwards) == -1 && rslt["result"] !== false;
                        if (hasValidActual) return false;
                    });
                    if (hasValidActual) { //strip maskforwards
                        results = $.map(results, function (rslt, ndx) {
                            if ($.inArray(rslt["activeMasksetIndex"], maskForwards) == -1) {
                                return rslt;
                            } else {
                                masksets[rslt["activeMasksetIndex"]]["lastValidPosition"] = actualLVP;
                            }
                        });
                    } else { //keep maskforwards with the least forward
                        var lowestPos = -1, lowestIndex = -1, rsltValid;
                        $.each(results, function (ndx, rslt) {
                            if ($.inArray(rslt["activeMasksetIndex"], maskForwards) != -1 && rslt["result"] !== false & (lowestPos == -1 || lowestPos > rslt["result"]["pos"])) {
                                lowestPos = rslt["result"]["pos"];
                                lowestIndex = rslt["activeMasksetIndex"];
                            }
                        });
                        results = $.map(results, function (rslt, ndx) {
                            if ($.inArray(rslt["activeMasksetIndex"], maskForwards) != -1) {
                                if (rslt["result"]["pos"] == lowestPos) {
                                    return rslt;
                                } else if (rslt["result"] !== false) {
                                    for (var i = pos; i < lowestPos; i++) {
                                        rsltValid = _isValid(i, masksets[rslt["activeMasksetIndex"]], masksets[lowestIndex]["buffer"][i], true);
                                        if (rsltValid === false) {
                                            masksets[rslt["activeMasksetIndex"]]["lastValidPosition"] = lowestPos - 1;
                                            break;
                                        } else {
                                            setBufferElement(masksets[rslt["activeMasksetIndex"]]["buffer"], i, masksets[lowestIndex]["buffer"][i], true);
                                            masksets[rslt["activeMasksetIndex"]]["lastValidPosition"] = i;
                                        }
                                    }
                                    //also check check for the lowestpos with the new input
                                    rsltValid = _isValid(lowestPos, masksets[rslt["activeMasksetIndex"]], c, true);
                                    if (rsltValid !== false) {
                                        setBufferElement(masksets[rslt["activeMasksetIndex"]]["buffer"], lowestPos, c, true);
                                        masksets[rslt["activeMasksetIndex"]]["lastValidPosition"] = lowestPos;
                                    }
                                    //console.log("ndx " + rslt["activeMasksetIndex"] + " validate " + masksets[rslt["activeMasksetIndex"]]["buffer"].join('') + " lv " + masksets[rslt["activeMasksetIndex"]]['lastValidPosition']);
                                    return rslt;
                                }
                            }
                        });
                    }
                    return results;
                }

                if (strict) {
                    var result = _isValid(pos, getActiveMaskSet(), c, strict); //only check validity in current mask when validating strict
                    if (result === true) {
                        result = { "pos": pos }; //always take a possible corrected maskposition into account
                    }
                    return result;
                }

                var results = [], result = false, currentActiveMasksetIndex = activeMasksetIndex,
                    actualBuffer = getActiveBuffer().slice(), actualLVP = getActiveMaskSet()["lastValidPosition"],
                    actualPrevious = seekPrevious(pos),
                    maskForwards = [];
                $.each(masksets, function (index, value) {
                    if (typeof (value) == "object") {
                        activeMasksetIndex = index;

                        var maskPos = pos;
                        var lvp = getActiveMaskSet()['lastValidPosition'],
                            rsltValid;
                        if (lvp == actualLVP) {
                            if ((maskPos - actualLVP) > 1) {
                                for (var i = lvp == -1 ? 0 : lvp; i < maskPos; i++) {
                                    rsltValid = _isValid(i, getActiveMaskSet(), actualBuffer[i], true);
                                    if (rsltValid === false) {
                                        break;
                                    } else {
                                        setBufferElement(getActiveBuffer(), i, actualBuffer[i], true);
                                        if (rsltValid === true) {
                                            rsltValid = { "pos": i }; //always take a possible corrected maskposition into account
                                        }
                                        var newValidPosition = rsltValid.pos || i;
                                        if (getActiveMaskSet()['lastValidPosition'] < newValidPosition)
                                            getActiveMaskSet()['lastValidPosition'] = newValidPosition; //set new position from isValid
                                    }
                                }
                            }
                            //does the input match on a further position?
                            if (!isMask(maskPos) && !_isValid(maskPos, getActiveMaskSet(), c, strict)) {
                                var maxForward = seekNext(maskPos) - maskPos;
                                for (var fw = 0; fw < maxForward; fw++) {
                                    if (_isValid(++maskPos, getActiveMaskSet(), c, strict) !== false)
                                        break;
                                }
                                maskForwards.push(activeMasksetIndex);
                                //console.log('maskforward ' + activeMasksetIndex + " pos " + pos + " maskPos " + maskPos);
                            }
                        }

                        if (getActiveMaskSet()['lastValidPosition'] >= actualLVP || activeMasksetIndex == currentActiveMasksetIndex) {
                            if (maskPos >= 0 && maskPos < getMaskLength()) {
                                result = _isValid(maskPos, getActiveMaskSet(), c, strict);
                                if (result !== false) {
                                    if (result === true) {
                                        result = { "pos": maskPos }; //always take a possible corrected maskposition into account
                                    }
                                    var newValidPosition = result.pos || maskPos;
                                    if (getActiveMaskSet()['lastValidPosition'] < newValidPosition)
                                        getActiveMaskSet()['lastValidPosition'] = newValidPosition; //set new position from isValid
                                }
                                //console.log("pos " + pos + " ndx " + activeMasksetIndex + " validate " + getActiveBuffer().join('') + " lv " + getActiveMaskSet()['lastValidPosition']);
                                results.push({ "activeMasksetIndex": index, "result": result });
                            }
                        }
                    }
                });
                activeMasksetIndex = currentActiveMasksetIndex; //reset activeMasksetIndex

                return PostProcessResults(maskForwards, results); //return results of the multiple mask validations
            }

            function determineActiveMasksetIndex() {
                var currentMasksetIndex = activeMasksetIndex,
                    highestValid = { "activeMasksetIndex": 0, "lastValidPosition": -1, "next": -1 };
                $.each(masksets, function (index, value) {
                    if (typeof (value) == "object") {
                        activeMasksetIndex = index;
                        if (getActiveMaskSet()['lastValidPosition'] > highestValid['lastValidPosition']) {
                            highestValid["activeMasksetIndex"] = index;
                            highestValid["lastValidPosition"] = getActiveMaskSet()['lastValidPosition'];
                            highestValid["next"] = seekNext(getActiveMaskSet()['lastValidPosition']);
                        } else if (getActiveMaskSet()['lastValidPosition'] == highestValid['lastValidPosition'] &&
                            (highestValid['next'] == -1 || highestValid['next'] > seekNext(getActiveMaskSet()['lastValidPosition']))) {
                            highestValid["activeMasksetIndex"] = index;
                            highestValid["lastValidPosition"] = getActiveMaskSet()['lastValidPosition'];
                            highestValid["next"] = seekNext(getActiveMaskSet()['lastValidPosition']);
                        }
                    }
                });

                activeMasksetIndex = highestValid["lastValidPosition"] != -1 && masksets[currentMasksetIndex]["lastValidPosition"] == highestValid["lastValidPosition"] ? currentMasksetIndex : highestValid["activeMasksetIndex"];
                if (currentMasksetIndex != activeMasksetIndex) {
                    clearBuffer(getActiveBuffer(), seekNext(highestValid["lastValidPosition"]), getMaskLength());
                    getActiveMaskSet()["writeOutBuffer"] = true;
                }
                $el.data('_inputmask')['activeMasksetIndex'] = activeMasksetIndex; //store the activeMasksetIndex
            }

            function isMask(pos) {
                var testPos = determineTestPosition(pos);
                var test = getActiveTests()[testPos];

                return test != undefined ? test.fn : false;
            }

            function determineTestPosition(pos) {
                return pos % getActiveTests().length;
            }

            function getMaskLength() {
                return opts.getMaskLength(getActiveBufferTemplate(), getActiveMaskSet()['greedy'], getActiveMaskSet()['repeat'], getActiveBuffer(), opts);
            }

            //pos: from position

            function seekNext(pos) {
                var maskL = getMaskLength();
                if (pos >= maskL) return maskL;
                var position = pos;
                while (++position < maskL && !isMask(position)) {
                }
                return position;
            }

            //pos: from position

            function seekPrevious(pos) {
                var position = pos;
                if (position <= 0) return 0;

                while (--position > 0 && !isMask(position)) {
                }
                ;
                return position;
            }

            function setBufferElement(buffer, position, element, autoPrepare) {
                if (autoPrepare) position = prepareBuffer(buffer, position);

                var test = getActiveTests()[determineTestPosition(position)];
                var elem = element;
                if (elem != undefined && test != undefined) {
                    switch (test.casing) {
                        case "upper":
                            elem = element.toUpperCase();
                            break;
                        case "lower":
                            elem = element.toLowerCase();
                            break;
                    }
                }

                buffer[position] = elem;
            }

            function getBufferElement(buffer, position, autoPrepare) {
                if (autoPrepare) position = prepareBuffer(buffer, position);
                return buffer[position];
            }

            //needed to handle the non-greedy mask repetitions

            function prepareBuffer(buffer, position) {
                var j;
                while (buffer[position] == undefined && buffer.length < getMaskLength()) {
                    j = 0;
                    while (getActiveBufferTemplate()[j] !== undefined) { //add a new buffer
                        buffer.push(getActiveBufferTemplate()[j++]);
                    }
                }

                return position;
            }

            function writeBuffer(input, buffer, caretPos) {
                input._valueSet(buffer.join(''));
                if (caretPos != undefined) {
                    caret(input, caretPos);
                }
            }

            function clearBuffer(buffer, start, end, stripNomasks) {
                for (var i = start, maskL = getMaskLength(); i < end && i < maskL; i++) {
                    if (stripNomasks === true) {
                        if (!isMask(i))
                            setBufferElement(buffer, i, "");
                    } else
                        setBufferElement(buffer, i, getBufferElement(getActiveBufferTemplate().slice(), i, true));
                }
            }

            function setReTargetPlaceHolder(buffer, pos) {
                var testPos = determineTestPosition(pos);
                setBufferElement(buffer, pos, getBufferElement(getActiveBufferTemplate(), testPos));
            }

            function getPlaceHolder(pos) {
                return opts.placeholder.charAt(pos % opts.placeholder.length);
            }

            function checkVal(input, writeOut, strict, nptvl, intelliCheck) {
                var inputValue = nptvl != undefined ? nptvl.slice() : truncateInput(input._valueGet()).split('');

                $.each(masksets, function (ndx, ms) {
                    if (typeof (ms) == "object") {
                        ms["buffer"] = ms["_buffer"].slice();
                        ms["lastValidPosition"] = -1;
                        ms["p"] = -1;
                    }
                });
                if (strict !== true) activeMasksetIndex = 0;
                if (writeOut) input._valueSet(""); //initial clear
                var ml = getMaskLength();
                $.each(inputValue, function (ndx, charCode) {
                    if (intelliCheck === true) {
                        var p = getActiveMaskSet()["p"], lvp = p == -1 ? p : seekPrevious(p),
                            pos = lvp == -1 ? ndx : seekNext(lvp);
                        if ($.inArray(charCode, getActiveBufferTemplate().slice(lvp + 1, pos)) == -1) {
                            keypressEvent.call(input, undefined, true, charCode.charCodeAt(0), writeOut, strict, ndx);
                        }
                    } else {
                        keypressEvent.call(input, undefined, true, charCode.charCodeAt(0), writeOut, strict, ndx);
                        strict = strict || (ndx > 0 && ndx > getActiveMaskSet()["p"]);
                    }
                });

                if (strict === true && getActiveMaskSet()["p"] != -1) {
                    getActiveMaskSet()["lastValidPosition"] = seekPrevious(getActiveMaskSet()["p"]);
                }
            }

            function escapeRegex(str) {
                return $.inputmask.escapeRegex.call(this, str);
            }

            function truncateInput(inputValue) {
                return inputValue.replace(new RegExp("(" + escapeRegex(getActiveBufferTemplate().join('')) + ")*$"), "");
            }

            function clearOptionalTail(input) {
                var buffer = getActiveBuffer(), tmpBuffer = buffer.slice(), testPos, pos;
                for (var pos = tmpBuffer.length - 1; pos >= 0; pos--) {
                    var testPos = determineTestPosition(pos);
                    if (getActiveTests()[testPos].optionality) {
                        if (!isMask(pos) || !isValid(pos, buffer[pos], true))
                            tmpBuffer.pop();
                        else break;
                    } else break;
                }
                writeBuffer(input, tmpBuffer);
            }

            function unmaskedvalue($input, skipDatepickerCheck) {
                if (getActiveTests() && (skipDatepickerCheck === true || !$input.hasClass('hasDatepicker'))) {
                    var umValue = $.map(getActiveBuffer(), function (element, index) {
                        return isMask(index) && isValid(index, element, true) ? element : null;
                    });
                    var unmaskedValue = (isRTL ? umValue.reverse() : umValue).join('');
                    return opts.onUnMask != undefined ? opts.onUnMask.call($input, getActiveBuffer().join(''), unmaskedValue, opts) : unmaskedValue;
                } else {
                    return $input[0]._valueGet();
                }
            }

            function TranslatePosition(pos) {
                if (isRTL && typeof pos == 'number' && (!opts.greedy || opts.placeholder != "")) {
                    var bffrLght = getActiveBuffer().length;
                    pos = bffrLght - pos;
                }
                return pos;
            }

            function caret(input, begin, end) {
                var npt = input.jquery && input.length > 0 ? input[0] : input, range;
                if (typeof begin == 'number') {
                    begin = TranslatePosition(begin);
                    end = TranslatePosition(end);
                    if (!$(npt).is(':visible')) {
                        return;
                    }
                    end = (typeof end == 'number') ? end : begin;
                    npt.scrollLeft = npt.scrollWidth;
                    if (opts.insertMode == false && begin == end) end++; //set visualization for insert/overwrite mode
                    if (npt.setSelectionRange) {
                        npt.selectionStart = begin;
                        npt.selectionEnd = android ? begin : end;

                    } else if (npt.createTextRange) {
                        range = npt.createTextRange();
                        range.collapse(true);
                        range.moveEnd('character', end);
                        range.moveStart('character', begin);
                        range.select();
                    }
                } else {
                    if (!$(input).is(':visible')) {
                        return { "begin": 0, "end": 0 };
                    }
                    if (npt.setSelectionRange) {
                        begin = npt.selectionStart;
                        end = npt.selectionEnd;
                    } else if (document.selection && document.selection.createRange) {
                        range = document.selection.createRange();
                        begin = 0 - range.duplicate().moveStart('character', -100000);
                        end = begin + range.text.length;
                    }
                    begin = TranslatePosition(begin);
                    end = TranslatePosition(end);
                    return { "begin": begin, "end": end };
                }
            }

            function isComplete(buffer) { //return true / false / undefined (repeat *)
                if (opts.repeat == "*") return undefined;
                var complete = false, highestValidPosition = 0, currentActiveMasksetIndex = activeMasksetIndex;
                $.each(masksets, function (ndx, ms) {
                    if (typeof (ms) == "object") {
                        activeMasksetIndex = ndx;
                        var aml = seekPrevious(getMaskLength());
                        if (ms["lastValidPosition"] >= highestValidPosition && ms["lastValidPosition"] == aml) {
                            var msComplete = true;
                            for (var i = 0; i <= aml; i++) {
                                var mask = isMask(i), testPos = determineTestPosition(i);
                                if ((mask && (buffer[i] == undefined || buffer[i] == getPlaceHolder(i))) || (!mask && buffer[i] != getActiveBufferTemplate()[testPos])) {
                                    msComplete = false;
                                    break;
                                }
                            }
                            complete = complete || msComplete;
                            if (complete) //break loop
                                return false;
                        }
                        highestValidPosition = ms["lastValidPosition"];
                    }
                });
                activeMasksetIndex = currentActiveMasksetIndex; //reset activeMaskset
                return complete;
            }

            function isSelection(begin, end) {
                return isRTL ? (begin - end) > 1 || ((begin - end) == 1 && opts.insertMode) :
                    (end - begin) > 1 || ((end - begin) == 1 && opts.insertMode);
            }


            //private functions
            function installEventRuler(npt) {
                var events = $._data(npt).events;

                $.each(events, function (eventType, eventHandlers) {
                    $.each(eventHandlers, function (ndx, eventHandler) {
                        if (eventHandler.namespace == "inputmask") {
                            if (eventHandler.type != "setvalue") {
                                var handler = eventHandler.handler;
                                eventHandler.handler = function (e) {
                                    if (this.readOnly || this.disabled)
                                        e.preventDefault;
                                    else
                                        return handler.apply(this, arguments);
                                };
                            }
                        }
                    });
                });
            }

            function patchValueProperty(npt) {
                function PatchValhook(type) {
                    if ($.valHooks[type] == undefined || $.valHooks[type].inputmaskpatch != true) {
                        var valueGet = $.valHooks[type] && $.valHooks[type].get ? $.valHooks[type].get : function (elem) { return elem.value; };
                        var valueSet = $.valHooks[type] && $.valHooks[type].set ? $.valHooks[type].set : function (elem, value) {
                            elem.value = value;
                            return elem;
                        };

                        $.valHooks[type] = {
                            get: function (elem) {
                                var $elem = $(elem);
                                if ($elem.data('_inputmask')) {
                                    if ($elem.data('_inputmask')['opts'].autoUnmask)
                                        return $elem.inputmask('unmaskedvalue');
                                    else {
                                        var result = valueGet(elem),
                                            inputData = $elem.data('_inputmask'), masksets = inputData['masksets'],
                                            activeMasksetIndex = inputData['activeMasksetIndex'];
                                        return result != masksets[activeMasksetIndex]['_buffer'].join('') ? result : '';
                                    }
                                } else return valueGet(elem);
                            },
                            set: function (elem, value) {
                                var $elem = $(elem);
                                var result = valueSet(elem, value);
                                if ($elem.data('_inputmask')) $elem.triggerHandler('setvalue.inputmask');
                                return result;
                            },
                            inputmaskpatch: true
                        };
                    }
                }
                var valueProperty;
                if (Object.getOwnPropertyDescriptor)
                    valueProperty = Object.getOwnPropertyDescriptor(npt, "value");
                if (valueProperty && valueProperty.get) {
                    if (!npt._valueGet) {
                        var valueGet = valueProperty.get;
                        var valueSet = valueProperty.set;
                        npt._valueGet = function () {
                            return isRTL ? valueGet.call(this).split('').reverse().join('') : valueGet.call(this);
                        };
                        npt._valueSet = function (value) {
                            valueSet.call(this, isRTL ? value.split('').reverse().join('') : value);
                        };

                        Object.defineProperty(npt, "value", {
                            get: function () {
                                var $self = $(this), inputData = $(this).data('_inputmask'), masksets = inputData['masksets'],
                                    activeMasksetIndex = inputData['activeMasksetIndex'];
                                return inputData && inputData['opts'].autoUnmask ? $self.inputmask('unmaskedvalue') : valueGet.call(this) != masksets[activeMasksetIndex]['_buffer'].join('') ? valueGet.call(this) : '';
                            },
                            set: function (value) {
                                valueSet.call(this, value);
                                $(this).triggerHandler('setvalue.inputmask');
                            }
                        });
                    }
                } else if (document.__lookupGetter__ && npt.__lookupGetter__("value")) {
                    if (!npt._valueGet) {
                        var valueGet = npt.__lookupGetter__("value");
                        var valueSet = npt.__lookupSetter__("value");
                        npt._valueGet = function () {
                            return isRTL ? valueGet.call(this).split('').reverse().join('') : valueGet.call(this);
                        };
                        npt._valueSet = function (value) {
                            valueSet.call(this, isRTL ? value.split('').reverse().join('') : value);
                        };

                        npt.__defineGetter__("value", function () {
                            var $self = $(this), inputData = $(this).data('_inputmask'), masksets = inputData['masksets'],
                                activeMasksetIndex = inputData['activeMasksetIndex'];
                            return inputData && inputData['opts'].autoUnmask ? $self.inputmask('unmaskedvalue') : valueGet.call(this) != masksets[activeMasksetIndex]['_buffer'].join('') ? valueGet.call(this) : '';
                        });
                        npt.__defineSetter__("value", function (value) {
                            valueSet.call(this, value);
                            $(this).triggerHandler('setvalue.inputmask');
                        });
                    }
                } else {
                    if (!npt._valueGet) {
                        npt._valueGet = function () { return isRTL ? this.value.split('').reverse().join('') : this.value; };
                        npt._valueSet = function (value) { this.value = isRTL ? value.split('').reverse().join('') : value; };
                    }
                    PatchValhook(npt.type);
                }
            }

            //shift chars to left from start to end and put c at end position if defined
            function shiftL(start, end, c, maskJumps) {
                var buffer = getActiveBuffer();
                if (maskJumps !== false) //jumping over nonmask position
                    while (!isMask(start) && start - 1 >= 0) start--;
                for (var i = start; i < end && i < getMaskLength(); i++) {
                    if (isMask(i)) {
                        setReTargetPlaceHolder(buffer, i);
                        var j = seekNext(i);
                        var p = getBufferElement(buffer, j);
                        if (p != getPlaceHolder(j)) {
                            if (j < getMaskLength() && isValid(i, p, true) !== false && getActiveTests()[determineTestPosition(i)].def == getActiveTests()[determineTestPosition(j)].def) {
                                setBufferElement(buffer, i, p, true);
                            } else {
                                if (isMask(i))
                                    break;
                            }
                        }
                    } else {
                        setReTargetPlaceHolder(buffer, i);
                    }
                }
                if (c != undefined)
                    setBufferElement(buffer, seekPrevious(end), c);

                if (getActiveMaskSet()["greedy"] == false) {
                    var trbuffer = truncateInput(buffer.join('')).split('');
                    buffer.length = trbuffer.length;
                    for (var i = 0, bl = buffer.length; i < bl; i++) {
                        buffer[i] = trbuffer[i];
                    }
                    if (buffer.length == 0) getActiveMaskSet()["buffer"] = getActiveBufferTemplate().slice();
                }
                return start; //return the used start position
            }

            function shiftR(start, end, c) {
                var buffer = getActiveBuffer();
                if (getBufferElement(buffer, start, true) != getPlaceHolder(start)) {
                    for (var i = seekPrevious(end); i > start && i >= 0; i--) {
                        if (isMask(i)) {
                            var j = seekPrevious(i);
                            var t = getBufferElement(buffer, j);
                            if (t != getPlaceHolder(j)) {
                                if (isValid(j, t, true) !== false && getActiveTests()[determineTestPosition(i)].def == getActiveTests()[determineTestPosition(j)].def) {
                                    setBufferElement(buffer, i, t, true);
                                    setReTargetPlaceHolder(buffer, j);
                                } //else break;
                            }
                        } else
                            setReTargetPlaceHolder(buffer, i);
                    }
                }
                if (c != undefined && getBufferElement(buffer, start) == getPlaceHolder(start))
                    setBufferElement(buffer, start, c);
                var lengthBefore = buffer.length;
                if (getActiveMaskSet()["greedy"] == false) {
                    var trbuffer = truncateInput(buffer.join('')).split('');
                    buffer.length = trbuffer.length;
                    for (var i = 0, bl = buffer.length; i < bl; i++) {
                        buffer[i] = trbuffer[i];
                    }
                    if (buffer.length == 0) getActiveMaskSet()["buffer"] = getActiveBufferTemplate().slice();
                }
                return end - (lengthBefore - buffer.length); //return new start position
            }

            ;


            function HandleRemove(input, k, pos) {
                if (opts.numericInput || isRTL) {
                    switch (k) {
                        case opts.keyCode.BACKSPACE:
                            k = opts.keyCode.DELETE;
                            break;
                        case opts.keyCode.DELETE:
                            k = opts.keyCode.BACKSPACE;
                            break;
                    }
                    if (isRTL) {
                        var pend = pos.end;
                        pos.end = pos.begin;
                        pos.begin = pend;
                    }
                }

                var isSelection = true;
                if (pos.begin == pos.end) {
                    var posBegin = k == opts.keyCode.BACKSPACE ? pos.begin - 1 : pos.begin;
                    if (opts.isNumeric && opts.radixPoint != "" && getActiveBuffer()[posBegin] == opts.radixPoint) {
                        pos.begin = (getActiveBuffer().length - 1 == posBegin) /* radixPoint is latest? delete it */ ? pos.begin : k == opts.keyCode.BACKSPACE ? posBegin : seekNext(posBegin);
                        pos.end = pos.begin;
                    }
                    isSelection = false;
                    if (k == opts.keyCode.BACKSPACE)
                        pos.begin--;
                    else if (k == opts.keyCode.DELETE)
                        pos.end++;
                } else if (pos.end - pos.begin == 1 && !opts.insertMode) {
                    isSelection = false;
                    if (k == opts.keyCode.BACKSPACE)
                        pos.begin--;
                }

                clearBuffer(getActiveBuffer(), pos.begin, pos.end);

                var ml = getMaskLength();
                if (opts.greedy == false) {
                    shiftL(pos.begin, ml, undefined, !isRTL && (k == opts.keyCode.BACKSPACE && !isSelection));
                } else {
                    var newpos = pos.begin;
                    for (var i = pos.begin; i < pos.end; i++) { //seeknext to skip placeholders at start in selection
                        if (isMask(i) || !isSelection)
                            newpos = shiftL(pos.begin, ml, undefined, !isRTL && (k == opts.keyCode.BACKSPACE && !isSelection));
                    }
                    if (!isSelection) pos.begin = newpos;
                }
                var firstMaskPos = seekNext(-1);
                clearBuffer(getActiveBuffer(), pos.begin, pos.end, true);
                checkVal(input, false, false, getActiveBuffer());
                if (getActiveMaskSet()['lastValidPosition'] < firstMaskPos) {
                    getActiveMaskSet()["lastValidPosition"] = -1;
                    getActiveMaskSet()["p"] = firstMaskPos;
                } else {
                    getActiveMaskSet()["p"] = pos.begin;
                }
            }

            function keydownEvent(e) {
                //Safari 5.1.x - modal dialog fires keypress twice workaround
                skipKeyPressEvent = false;
                var input = this, $input = $(input), k = e.keyCode, pos = caret(input);

                //backspace, delete, and escape get special treatment
                if (k == opts.keyCode.BACKSPACE || k == opts.keyCode.DELETE || (iphone && k == 127) || e.ctrlKey && k == 88) { //backspace/delete
                    e.preventDefault(); //stop default action but allow propagation
                    if (k == 88) valueOnFocus = getActiveBuffer().join('');
                    HandleRemove(input, k, pos);
                    determineActiveMasksetIndex();
                    writeBuffer(input, getActiveBuffer(), getActiveMaskSet()["p"]);
                    if (input._valueGet() == getActiveBufferTemplate().join(''))
                        $input.trigger('cleared');

                    if (opts.showTooltip) { //update tooltip
                        $input.prop("title", getActiveMaskSet()["mask"]);
                    }
                } else if (k == opts.keyCode.END || k == opts.keyCode.PAGE_DOWN) { //when END or PAGE_DOWN pressed set position at lastmatch
                    setTimeout(function () {
                        var caretPos = seekNext(getActiveMaskSet()["lastValidPosition"]);
                        if (!opts.insertMode && caretPos == getMaskLength() && !e.shiftKey) caretPos--;
                        caret(input, e.shiftKey ? pos.begin : caretPos, caretPos);
                    }, 0);
                } else if ((k == opts.keyCode.HOME && !e.shiftKey) || k == opts.keyCode.PAGE_UP) { //Home or page_up
                    caret(input, 0, e.shiftKey ? pos.begin : 0);
                } else if (k == opts.keyCode.ESCAPE || (k == 90 && e.ctrlKey)) { //escape && undo
                    checkVal(input, true, false, valueOnFocus.split(''));
                    $input.click();
                } else if (k == opts.keyCode.INSERT && !(e.shiftKey || e.ctrlKey)) { //insert
                    opts.insertMode = !opts.insertMode;
                    caret(input, !opts.insertMode && pos.begin == getMaskLength() ? pos.begin - 1 : pos.begin);
                } else if (opts.insertMode == false && !e.shiftKey) {
                    if (k == opts.keyCode.RIGHT) {
                        setTimeout(function () {
                            var caretPos = caret(input);
                            caret(input, caretPos.begin);
                        }, 0);
                    } else if (k == opts.keyCode.LEFT) {
                        setTimeout(function () {
                            var caretPos = caret(input);
                            caret(input, caretPos.begin - 1);
                        }, 0);
                    }
                }

                var currentCaretPos = caret(input);
                if (opts.onKeyDown.call(this, e, getActiveBuffer(), opts) === true) //extra stuff to execute on keydown
                    caret(input, currentCaretPos.begin, currentCaretPos.end);
                ignorable = $.inArray(k, opts.ignorables) != -1;
            }


            function keypressEvent(e, checkval, k, writeOut, strict, ndx) {
                //Safari 5.1.x - modal dialog fires keypress twice workaround
                if (k == undefined && skipKeyPressEvent) return false;
                skipKeyPressEvent = true;

                var input = this, $input = $(input);

                e = e || window.event;
                var k = checkval ? k : (e.which || e.charCode || e.keyCode);

                if (checkval !== true && (!(e.ctrlKey && e.altKey) && (e.ctrlKey || e.metaKey || ignorable))) {
                    return true;
                } else {
                    if (k) {
                        //special treat the decimal separator
                        if (checkval !== true && k == 46 && e.shiftKey == false && opts.radixPoint == ",") k = 44;

                        var pos, results, result, c = String.fromCharCode(k);
                        if (checkval) {
                            var pcaret = strict ? ndx : getActiveMaskSet()["lastValidPosition"] + 1;
                            pos = { begin: pcaret, end: pcaret };
                        } else {
                            pos = caret(input);
                        }

                        //should we clear a possible selection??
                        var isSlctn = isSelection(pos.begin, pos.end),
                            initialIndex = activeMasksetIndex;
                        if (isSlctn) {
                            $.each(masksets, function (ndx, lmnt) { //init undobuffer for recovery when not valid
                                if (typeof (lmnt) == "object") {
                                    activeMasksetIndex = ndx;
                                    getActiveMaskSet()["undoBuffer"] = getActiveBuffer().join('');
                                }
                            });
                            activeMasksetIndex = initialIndex; //restore index
                            HandleRemove(input, opts.keyCode.DELETE, pos);
                            if (!opts.insertMode) { //preserve some space
                                $.each(masksets, function (ndx, lmnt) {
                                    if (typeof (lmnt) == "object") {
                                        activeMasksetIndex = ndx;
                                        shiftR(pos.begin, getMaskLength());
                                        getActiveMaskSet()["lastValidPosition"] = seekNext(getActiveMaskSet()["lastValidPosition"]);
                                    }
                                });
                            }
                            activeMasksetIndex = initialIndex; //restore index
                        }

                        var radixPosition = getActiveBuffer().join('').indexOf(opts.radixPoint);
                        if (opts.isNumeric && checkval !== true && radixPosition != -1) {
                            if (opts.greedy && pos.begin <= radixPosition) {
                                pos.begin = seekPrevious(pos.begin);
                                pos.end = pos.begin;
                            } else if (c == opts.radixPoint) {
                                pos.begin = radixPosition;
                                pos.end = pos.begin;
                            }
                        }


                        var p = pos.begin;
                        results = isValid(p, c, strict);
                        if (strict === true) results = [{ "activeMasksetIndex": activeMasksetIndex, "result": results}];
                        var minimalForwardPosition = -1;
                        $.each(results, function (index, result) {
                            activeMasksetIndex = result["activeMasksetIndex"];
                            getActiveMaskSet()["writeOutBuffer"] = true;
                            var np = result["result"];
                            if (np !== false) {
                                var refresh = false, buffer = getActiveBuffer();
                                if (np !== true) {
                                    refresh = np["refresh"]; //only rewrite buffer from isValid
                                    p = np.pos != undefined ? np.pos : p; //set new position from isValid
                                    c = np.c != undefined ? np.c : c; //set new char from isValid
                                }
                                if (refresh !== true) {
                                    if (opts.insertMode == true) {
                                        var lastUnmaskedPosition = getMaskLength();
                                        var bfrClone = buffer.slice();
                                        while (getBufferElement(bfrClone, lastUnmaskedPosition, true) != getPlaceHolder(lastUnmaskedPosition) && lastUnmaskedPosition >= p) {
                                            lastUnmaskedPosition = lastUnmaskedPosition == 0 ? -1 : seekPrevious(lastUnmaskedPosition);
                                        }
                                        if (lastUnmaskedPosition >= p) {
                                            shiftR(p, getMaskLength(), c);
                                            //shift the lvp if needed
                                            var lvp = getActiveMaskSet()["lastValidPosition"], nlvp = seekNext(lvp);
                                            if (nlvp != getMaskLength() && lvp >= p && (getBufferElement(getActiveBuffer().slice(), nlvp, true) != getPlaceHolder(nlvp))) {
                                                getActiveMaskSet()["lastValidPosition"] = nlvp;
                                            }
                                        } else getActiveMaskSet()["writeOutBuffer"] = false;
                                    } else setBufferElement(buffer, p, c, true);
                                    if (minimalForwardPosition == -1 || minimalForwardPosition > seekNext(p)) {
                                        minimalForwardPosition = seekNext(p);
                                    }
                                } else if (!strict) {
                                    var nextPos = p < getMaskLength() ? p + 1 : p;
                                    if (minimalForwardPosition == -1 || minimalForwardPosition > nextPos) {
                                        minimalForwardPosition = nextPos;
                                    }
                                }
                                if (minimalForwardPosition > getActiveMaskSet()["p"])
                                    getActiveMaskSet()["p"] = minimalForwardPosition; //needed for checkval strict 
                            }
                        });

                        if (strict !== true) {
                            activeMasksetIndex = initialIndex;
                            determineActiveMasksetIndex();
                        }
                        if (writeOut !== false) {
                            $.each(results, function (ndx, rslt) {
                                if (rslt["activeMasksetIndex"] == activeMasksetIndex) {
                                    result = rslt;
                                    return false;
                                }
                            });
                            if (result != undefined) {
                                var self = this;
                                setTimeout(function () { opts.onKeyValidation.call(self, result["result"], opts); }, 0);
                                if (getActiveMaskSet()["writeOutBuffer"] && result["result"] !== false) {
                                    var buffer = getActiveBuffer();

                                    var newCaretPosition;
                                    if (checkval) {
                                        newCaretPosition = undefined;
                                    } else if (opts.numericInput) {
                                        if (p > radixPosition) {
                                            newCaretPosition = seekPrevious(minimalForwardPosition);
                                        } else if (c == opts.radixPoint) {
                                            newCaretPosition = minimalForwardPosition - 1;
                                        } else newCaretPosition = seekPrevious(minimalForwardPosition - 1);
                                    } else {
                                        newCaretPosition = minimalForwardPosition;
                                    }

                                    writeBuffer(input, buffer, newCaretPosition);
                                    if (checkval !== true) {
                                        setTimeout(function () { //timeout needed for IE
                                            if (isComplete(buffer) === true)
                                                $input.trigger("complete");
                                            skipInputEvent = true;
                                            $input.trigger("input");
                                        }, 0);
                                    }
                                } else if (isSlctn) {
                                    getActiveMaskSet()["buffer"] = getActiveMaskSet()["undoBuffer"].split('');
                                }
                            }
                        }

                        if (opts.showTooltip) { //update tooltip
                            $input.prop("title", getActiveMaskSet()["mask"]);
                        }

                        //needed for IE8 and below
                        if (e) e.preventDefault ? e.preventDefault() : e.returnValue = false;
                    }
                }
            }

            function keyupEvent(e) {
                var $input = $(this), input = this, k = e.keyCode, buffer = getActiveBuffer();

                opts.onKeyUp.call(this, e, buffer, opts); //extra stuff to execute on keyup
                if (k == opts.keyCode.TAB && opts.showMaskOnFocus) {
                    if ($input.hasClass('focus.inputmask') && input._valueGet().length == 0) {
                        buffer = getActiveBufferTemplate().slice();
                        writeBuffer(input, buffer);
                        caret(input, 0);
                        valueOnFocus = getActiveBuffer().join('');
                        comsole.log("sss");
                    } else {
                        writeBuffer(input, buffer);
                        if (buffer.join('') == getActiveBufferTemplate().join('') && $.inArray(opts.radixPoint, buffer) != -1) {
                            caret(input, TranslatePosition(0));
                            $input.click();
                        } else
                            caret(input, TranslatePosition(0), TranslatePosition(getMaskLength()));
                    }
                }
            }

            function pasteEvent(e) {
                if (skipInputEvent === true && e.type == "input") {
                    skipInputEvent = false;
                    return true;
                }

                var input = this, $input = $(input);
                //paste event for IE8 and lower I guess ;-)
                if (e.type == "propertychange" && input._valueGet().length <= getMaskLength()) {
                    return true;
                }
                setTimeout(function () {
                    var pasteValue = opts.onBeforePaste != undefined ? opts.onBeforePaste.call(input, input._valueGet(), opts) : input._valueGet();
                    checkVal(input, false, false, pasteValue.split(''), true);
                    writeBuffer(input, getActiveBuffer());
                    if (isComplete(getActiveBuffer()) === true)
                        $input.trigger("complete");
                    $input.click();
                }, 0);
            }

            function mobileInputEvent(e) {
                var input = this, $input = $(input);

                //backspace in chrome32 only fires input event - detect & treat
                var caretPos = caret(input),
                    currentValue = input._valueGet();

                if ((getActiveBuffer().length - currentValue.length) == 1 && currentValue.charAt(caretPos.begin) != getActiveBuffer()[caretPos.begin]
                    && currentValue.charAt(caretPos.begin + 1) != getActiveBuffer()[caretPos.begin]
                    && !isMask(caretPos.begin)) {
                    e.keyCode = opts.keyCode.BACKSPACE;
                    keydownEvent.call(input, e);
                } else { //nonnumerics don't fire keypress 
                    checkVal(input, false, false);
                    writeBuffer(input, getActiveBuffer());
                    if (isComplete(getActiveBuffer()) === true)
                        $input.trigger("complete");
                    $input.click();
                }
                e.preventDefault();
            }

            function mask(el) {
                $el = $(el);
                if ($el.is(":input")) {
                    //store tests & original buffer in the input element - used to get the unmasked value
                    $el.data('_inputmask', {
                        'masksets': masksets,
                        'activeMasksetIndex': activeMasksetIndex,
                        'opts': opts,
                        'isRTL': false
                    });

                    //show tooltip
                    if (opts.showTooltip) {
                        $el.prop("title", getActiveMaskSet()["mask"]);
                    }

                    //correct greedy setting if needed
                    getActiveMaskSet()['greedy'] = getActiveMaskSet()['greedy'] ? getActiveMaskSet()['greedy'] : getActiveMaskSet()['repeat'] == 0;

                    //handle maxlength attribute
                    if ($el.attr("maxLength") != null) //only when the attribute is set
                    {
                        var maxLength = $el.prop('maxLength');
                        if (maxLength > -1) { //handle *-repeat
                            $.each(masksets, function (ndx, ms) {
                                if (typeof (ms) == "object") {
                                    if (ms["repeat"] == "*") {
                                        ms["repeat"] = maxLength;
                                    }
                                }
                            });
                        }
                        if (getMaskLength() >= maxLength && maxLength > -1) { //FF sets no defined max length to -1 
                            if (maxLength < getActiveBufferTemplate().length) getActiveBufferTemplate().length = maxLength;
                            if (getActiveMaskSet()['greedy'] == false) {
                                getActiveMaskSet()['repeat'] = Math.round(maxLength / getActiveBufferTemplate().length);
                            }
                            $el.prop('maxLength', getMaskLength() * 2);
                        }
                    }

                    patchValueProperty(el);

                    if (opts.numericInput) opts.isNumeric = opts.numericInput;
                    if (el.dir == "rtl" || (opts.numericInput && opts.rightAlignNumerics) || (opts.isNumeric && opts.rightAlignNumerics))
                        $el.css("text-align", "right");

                    if (el.dir == "rtl" || opts.numericInput) {
                        el.dir = "ltr";
                        $el.removeAttr("dir");
                        var inputData = $el.data('_inputmask');
                        inputData['isRTL'] = true;
                        $el.data('_inputmask', inputData);
                        isRTL = true;
                    }

                    //unbind all events - to make sure that no other mask will interfere when re-masking
                    $el.unbind(".inputmask");
                    $el.removeClass('focus.inputmask');
                    //bind events
                    $el.closest('form').bind("submit", function () { //trigger change on submit if any
                        if (valueOnFocus != getActiveBuffer().join('')) {
                            $el.change();
                        }
                    }).bind('reset', function () {
                        setTimeout(function () {
                            $el.trigger("setvalue");
                        }, 0);
                    });
                    $el.bind("mouseenter.inputmask", function () {
                        var $input = $(this), input = this;
                        if (!$input.hasClass('focus.inputmask') && opts.showMaskOnHover) {
                            //console.log("1");
                            if (input._valueGet() != getActiveBuffer().join('')) {
                                writeBuffer(input, getActiveBuffer());
                                //console.log("2");
                            }
                        }
                    }).bind("blur.inputmask", function () {
                        var $input = $(this), input = this, nptValue = input._valueGet(), buffer = getActiveBuffer();
                        $input.removeClass('focus.inputmask');
                        if (valueOnFocus != getActiveBuffer().join('')) {
                            $input.change();
                        }
                        if (opts.clearMaskOnLostFocus && nptValue != '') {
                            
 
                            if (nptValue == getActiveBufferTemplate().join('')){
                                input._valueSet('');
                                //console.log($.now(), "111111111111111111");
                            }else { //clearout optional tail of the mask
                                //clearOptionalTail(input);
                                //console.log($.now(), "222222222222222222");
                            }
                        }
                        if (isComplete(buffer) === false) {
                            $input.trigger("incomplete");
                            if (opts.clearIncomplete) {
                                $.each(masksets, function (ndx, ms) {
                                    if (typeof (ms) == "object") {
                                        ms["buffer"] = ms["_buffer"].slice();
                                        ms["lastValidPosition"] = -1;
                                    }
                                });
                                activeMasksetIndex = 0;
                                if (opts.clearMaskOnLostFocus)
                                    input._valueSet('');
                                else {
                                    buffer = getActiveBufferTemplate().slice();
                                    writeBuffer(input, buffer);
                                }
                            }
                        }
                    }).bind("focus.inputmask", function () {
                        var $input = $(this), input = this, nptValue = input._valueGet();
                        if (opts.showMaskOnFocus && !$input.hasClass('focus.inputmask') && (!opts.showMaskOnHover || (opts.showMaskOnHover && nptValue == ''))) {
                            if (input._valueGet() != getActiveBuffer().join('')) {
                                writeBuffer(input, getActiveBuffer(), seekNext(getActiveMaskSet()["lastValidPosition"]));
                            }
                        }
                        $input.addClass('focus.inputmask');
                        valueOnFocus = getActiveBuffer().join('');
                    }).bind("mouseleave.inputmask", function () {
                        var $input = $(this), input = this;
                        if (opts.clearMaskOnLostFocus) {
                            if (!$input.hasClass('focus.inputmask') && input._valueGet() != $input.attr("placeholder")) {
                                if (input._valueGet() == getActiveBufferTemplate().join('') || input._valueGet() == '')
                                    input._valueSet('');
                                else { //clearout optional tail of the mask
                                    //clearOptionalTail(input);
                                }
                            }
                        }
                    }).bind("click.inputmask", function () {
                        var input = this;
                        setTimeout(function () {
                            var selectedCaret = caret(input), buffer = getActiveBuffer();
                            if (selectedCaret.begin == selectedCaret.end) {
                                var clickPosition = isRTL ? TranslatePosition(selectedCaret.begin) : selectedCaret.begin,
                                    lvp = getActiveMaskSet()["lastValidPosition"],
                                    lastPosition;
                                if (opts.isNumeric) {
                                    lastPosition = opts.skipRadixDance === false && opts.radixPoint != "" && $.inArray(opts.radixPoint, buffer) != -1 ?
                                        (opts.numericInput ? seekNext($.inArray(opts.radixPoint, buffer)) : $.inArray(opts.radixPoint, buffer)) :
                                        seekNext(lvp);
                                } else {
                                    lastPosition = seekNext(lvp);
                                }
                                if (clickPosition < lastPosition) {
                                    if (isMask(clickPosition))
                                        caret(input, clickPosition);
                                    else caret(input, seekNext(clickPosition));
                                } else
                                    caret(input, lastPosition);
                            }
                        }, 0);
                    }).bind('dblclick.inputmask', function () {
                        var input = this;
                        setTimeout(function () {
                            caret(input, 0, seekNext(getActiveMaskSet()["lastValidPosition"]));
                        }, 0);
                    }).bind(PasteEventType + ".inputmask dragdrop.inputmask drop.inputmask", pasteEvent
                    ).bind('setvalue.inputmask', function () {
                        var input = this;
                        checkVal(input, true);
                        valueOnFocus = getActiveBuffer().join('');
                        if (input._valueGet() == getActiveBufferTemplate().join(''))
                            input._valueSet('');
                    }).bind('complete.inputmask', opts.oncomplete
                    ).bind('incomplete.inputmask', opts.onincomplete
                    ).bind('cleared.inputmask', opts.oncleared);

                    $el.bind("keydown.inputmask", keydownEvent
                         ).bind("keypress.inputmask", keypressEvent
                         ).bind("keyup.inputmask", keyupEvent);

                    // as the other inputevents aren't reliable for the moment we only base on the input event
                    // needs follow-up
                    if (android || androidfirefox || androidchrome) {
                        $el.unbind("keydown.inputmask", keydownEvent
                         	).unbind("keypress.inputmask", keypressEvent
                         	).unbind("keyup.inputmask", keyupEvent);
                        if (PasteEventType == "input") {
                            $el.unbind(PasteEventType + ".inputmask");
                        }
                        $el.bind("input.inputmask", mobileInputEvent);
                    }

                    if (msie1x)
                        $el.bind("input.inputmask", pasteEvent);

                    //apply mask
                    var initialValue = opts.onBeforeMask != undefined ? opts.onBeforeMask.call(el, el._valueGet(), opts) : el._valueGet();
                    checkVal(el, true, false, initialValue.split(''));
                    valueOnFocus = getActiveBuffer().join('');
                    // Wrap document.activeElement in a try/catch block since IE9 throw "Unspecified error" if document.activeElement is undefined when we are in an IFrame.
                    var activeElement;
                    try {
                        activeElement = document.activeElement;
                    } catch (e) {
                    }
                    if (activeElement === el) { //position the caret when in focus
                        $el.addClass('focus.inputmask');
                        caret(el, seekNext(getActiveMaskSet()["lastValidPosition"]));
                    } else if (opts.clearMaskOnLostFocus) {
                        if (getActiveBuffer().join('') == getActiveBufferTemplate().join('')) {
                            el._valueSet('');
                        } else {
                            clearOptionalTail(el);
                        }
                    } else {
                        writeBuffer(el, getActiveBuffer());
                    }

                    installEventRuler(el);
                }
            }

            //action object
            if (actionObj != undefined) {
                switch (actionObj["action"]) {
                    case "isComplete":
                        return isComplete(actionObj["buffer"]);
                    case "unmaskedvalue":
                        isRTL = actionObj["$input"].data('_inputmask')['isRTL'];
                        return unmaskedvalue(actionObj["$input"], actionObj["skipDatepickerCheck"]);
                    case "mask":
                        mask(actionObj["el"]);
                        break;
                    case "format":
                        $el = $({});
                        $el.data('_inputmask', {
                            'masksets': masksets,
                            'activeMasksetIndex': activeMasksetIndex,
                            'opts': opts,
                            'isRTL': opts.numericInput
                        });
                        if (opts.numericInput) {
                            opts.isNumeric = opts.numericInput;
                            isRTL = true;
                        }

                        checkVal($el, false, false, actionObj["value"].split(''), true);
                        return getActiveBuffer().join('');
                    case "isValid":
                        $el = $({});
                        $el.data('_inputmask', {
                            'masksets': masksets,
                            'activeMasksetIndex': activeMasksetIndex,
                            'opts': opts,
                            'isRTL': opts.numericInput
                        });
                        if (opts.numericInput) {
                            opts.isNumeric = opts.numericInput;
                            isRTL = true;
                        }

                        checkVal($el, false, true, actionObj["value"].split(''));
                        return isComplete(getActiveBuffer());
                }
            }
        };

        $.inputmask = {
            //options default
            defaults: {
                placeholder: "_",
                optionalmarker: { start: "[", end: "]" },
                quantifiermarker: { start: "{", end: "}" },
                groupmarker: { start: "(", end: ")" },
                escapeChar: "\\",
                mask: null,
                oncomplete: $.noop, //executes when the mask is complete
                onincomplete: $.noop, //executes when the mask is incomplete and focus is lost
                oncleared: $.noop, //executes when the mask is cleared
                repeat: 0, //repetitions of the mask: * ~ forever, otherwise specify an integer
                greedy: true, //true: allocated buffer for the mask and repetitions - false: allocate only if needed
                autoUnmask: false, //automatically unmask when retrieving the value with $.fn.val or value if the browser supports __lookupGetter__ or getOwnPropertyDescriptor
                clearMaskOnLostFocus: true,
                insertMode: true, //insert the input or overwrite the input
                clearIncomplete: false, //clear the incomplete input on blur
                aliases: {}, //aliases definitions => see jquery.inputmask.extensions.js
                onKeyUp: $.noop, //override to implement autocomplete on certain keys for example
                onKeyDown: $.noop, //override to implement autocomplete on certain keys for example
                onBeforeMask: undefined, //executes before masking the initial value to allow preprocessing of the initial value.  args => initialValue, opts => return processedValue
                onBeforePaste: undefined, //executes before masking the pasted value to allow preprocessing of the pasted value.  args => pastedValue, opts => return processedValue
                onUnMask: undefined, //executes after unmasking to allow postprocessing of the unmaskedvalue.  args => maskedValue, unmaskedValue, opts
                showMaskOnFocus: false, //show the mask-placeholder when the input has focus
                showMaskOnHover: false, //show the mask-placeholder when hovering the empty input
                onKeyValidation: $.noop, //executes on every key-press with the result of isValid. Params: result, opts
                skipOptionalPartCharacter: " ", //a character which can be used to skip an optional part of a mask
                showTooltip: false, //show the activemask as tooltip
                numericInput: false, //numericInput input direction style (input shifts to the left while holding the caret position)
                //numeric basic properties
                isNumeric: false, //enable numeric features
                radixPoint: "", //".", // | ","
                skipRadixDance: false, //disable radixpoint caret positioning
                rightAlignNumerics: true, //align numerics to the right
                //numeric basic properties
                definitions: {
                    '9': {
                        validator: "[0-9]",
                        cardinality: 1,
                        definitionSymbol: "*"
                    },
                    'a': {
                        validator: "[A-Za-z\u0410-\u044F\u0401\u0451]",
                        cardinality: 1,
                        definitionSymbol: "*"
                    },
                    '*': {
                        validator: "[A-Za-z\u0410-\u044F\u0401\u04510-9]",
                        cardinality: 1
                    }
                },
                keyCode: {
                    ALT: 18, BACKSPACE: 8, CAPS_LOCK: 20, COMMA: 188, COMMAND: 91, COMMAND_LEFT: 91, COMMAND_RIGHT: 93, CONTROL: 17, DELETE: 46, DOWN: 40, END: 35, ENTER: 13, ESCAPE: 27, HOME: 36, INSERT: 45, LEFT: 37, MENU: 93, NUMPAD_ADD: 107, NUMPAD_DECIMAL: 110, NUMPAD_DIVIDE: 111, NUMPAD_ENTER: 108,
                    NUMPAD_MULTIPLY: 106, NUMPAD_SUBTRACT: 109, PAGE_DOWN: 34, PAGE_UP: 33, PERIOD: 190, RIGHT: 39, SHIFT: 16, SPACE: 32, TAB: 9, UP: 38, WINDOWS: 91
                },
                //specify keycodes which should not be considered in the keypress event, otherwise the preventDefault will stop their default behavior especially in FF
                ignorables: [8, 9, 13, 19, 27, 33, 34, 35, 36, 37, 38, 39, 40, 45, 46, 93, 112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123],
                getMaskLength: function (buffer, greedy, repeat, currentBuffer, opts) {
                    var calculatedLength = buffer.length;
                    if (!greedy) {
                        if (repeat == "*") {
                            calculatedLength = currentBuffer.length + 1;
                        } else if (repeat > 1) {
                            calculatedLength += (buffer.length * (repeat - 1));
                        }
                    }
                    return calculatedLength;
                }
            },
            escapeRegex: function (str) {
                var specials = ['/', '.', '*', '+', '?', '|', '(', ')', '[', ']', '{', '}', '\\'];
                return str.replace(new RegExp('(\\' + specials.join('|\\') + ')', 'gim'), '\\$1');
            },
            format: function (value, options) {
                var opts = $.extend(true, {}, $.inputmask.defaults, options);
                resolveAlias(opts.alias, options, opts);
                return maskScope(generateMaskSets(opts), 0, opts, { "action": "format", "value": value });
            },
            isValid: function (value, options) {
                var opts = $.extend(true, {}, $.inputmask.defaults, options);
                resolveAlias(opts.alias, options, opts);
                return maskScope(generateMaskSets(opts), 0, opts, { "action": "isValid", "value": value });
            }
        };

        $.fn.inputmask = function (fn, options) {
            var opts = $.extend(true, {}, $.inputmask.defaults, options),
                masksets,
                activeMasksetIndex = 0;

            if (typeof fn === "string") {
                switch (fn) {
                    case "mask":
                        //resolve possible aliases given by options
                        resolveAlias(opts.alias, options, opts);
                        masksets = generateMaskSets(opts);
                        if (masksets.length == 0) { return this; }

                        return this.each(function () {
                            maskScope($.extend(true, {}, masksets), 0, opts, { "action": "mask", "el": this });
                        });
                    case "unmaskedvalue":
                        var $input = $(this), input = this;
                        if ($input.data('_inputmask')) {
                            masksets = $input.data('_inputmask')['masksets'];
                            activeMasksetIndex = $input.data('_inputmask')['activeMasksetIndex'];
                            opts = $input.data('_inputmask')['opts'];
                            return maskScope(masksets, activeMasksetIndex, opts, { "action": "unmaskedvalue", "$input": $input });
                        } else return $input.val();
                    case "remove":
                        return this.each(function () {
                            var $input = $(this), input = this;
                            if ($input.data('_inputmask')) {
                                masksets = $input.data('_inputmask')['masksets'];
                                activeMasksetIndex = $input.data('_inputmask')['activeMasksetIndex'];
                                opts = $input.data('_inputmask')['opts'];
                                //writeout the unmaskedvalue
                                input._valueSet(maskScope(masksets, activeMasksetIndex, opts, { "action": "unmaskedvalue", "$input": $input, "skipDatepickerCheck": true }));
                                //clear data
                                $input.removeData('_inputmask');
                                //unbind all events
                                $input.unbind(".inputmask");
                                $input.removeClass('focus.inputmask');
                                //restore the value property
                                var valueProperty;
                                if (Object.getOwnPropertyDescriptor)
                                    valueProperty = Object.getOwnPropertyDescriptor(input, "value");
                                if (valueProperty && valueProperty.get) {
                                    if (input._valueGet) {
                                        Object.defineProperty(input, "value", {
                                            get: input._valueGet,
                                            set: input._valueSet
                                        });
                                    }
                                } else if (document.__lookupGetter__ && input.__lookupGetter__("value")) {
                                    if (input._valueGet) {
                                        input.__defineGetter__("value", input._valueGet);
                                        input.__defineSetter__("value", input._valueSet);
                                    }
                                }
                                try { //try catch needed for IE7 as it does not supports deleting fns
                                    delete input._valueGet;
                                    delete input._valueSet;
                                } catch (e) {
                                    input._valueGet = undefined;
                                    input._valueSet = undefined;

                                }
                            }
                        });
                        break;
                    case "getemptymask": //return the default (empty) mask value, usefull for setting the default value in validation
                        if (this.data('_inputmask')) {
                            masksets = this.data('_inputmask')['masksets'];
                            activeMasksetIndex = this.data('_inputmask')['activeMasksetIndex'];
                            return masksets[activeMasksetIndex]['_buffer'].join('');
                        }
                        else return "";
                    case "hasMaskedValue": //check wheter the returned value is masked or not; currently only works reliable when using jquery.val fn to retrieve the value 
                        return this.data('_inputmask') ? !this.data('_inputmask')['opts'].autoUnmask : false;
                    case "isComplete":
                        masksets = this.data('_inputmask')['masksets'];
                        activeMasksetIndex = this.data('_inputmask')['activeMasksetIndex'];
                        opts = this.data('_inputmask')['opts'];
                        return maskScope(masksets, activeMasksetIndex, opts, { "action": "isComplete", "buffer": this[0]._valueGet().split('') });
                    case "getmetadata": //return mask metadata if exists
                        if (this.data('_inputmask')) {
                            masksets = this.data('_inputmask')['masksets'];
                            activeMasksetIndex = this.data('_inputmask')['activeMasksetIndex'];
                            return masksets[activeMasksetIndex]['metadata'];
                        }
                        else return undefined;
                    default:
                        //check if the fn is an alias
                        if (!resolveAlias(fn, options, opts)) {
                            //maybe fn is a mask so we try
                            //set mask
                            opts.mask = fn;
                        }
                        masksets = generateMaskSets(opts);
                        if (masksets.length == 0) { return this; }
                        return this.each(function () {
                            maskScope($.extend(true, {}, masksets), activeMasksetIndex, opts, { "action": "mask", "el": this });
                        });

                        break;
                }
            } else if (typeof fn == "object") {
                opts = $.extend(true, {}, $.inputmask.defaults, fn);

                resolveAlias(opts.alias, fn, opts); //resolve aliases
                masksets = generateMaskSets(opts);
                if (masksets.length == 0) { return this; }
                return this.each(function () {
                    maskScope($.extend(true, {}, masksets), activeMasksetIndex, opts, { "action": "mask", "el": this });
                });
            } else if (fn == undefined) {
                //look for data-inputmask atribute - the attribute should only contain optipns
                return this.each(function () {
                    var attrOptions = $(this).attr("data-inputmask");
                    if (attrOptions && attrOptions != "") {
                        try {
                            attrOptions = attrOptions.replace(new RegExp("'", "g"), '"');
                            var dataoptions = $.parseJSON("{" + attrOptions + "}");
                            $.extend(true, dataoptions, options);
                            opts = $.extend(true, {}, $.inputmask.defaults, dataoptions);
                            resolveAlias(opts.alias, dataoptions, opts);
                            opts.alias = undefined;
                            $(this).inputmask(opts);
                        } catch (ex) { } //need a more relax parseJSON
                    }
                });
            }
        };
    }
})(jQuery);
/*
Input Mask plugin extensions
http://github.com/RobinHerbots/jquery.inputmask
Copyright (c) 2010 - 2014 Robin Herbots
Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
Version: 2.5.0

Optional extensions on the jquery.inputmask base
*/
(function ($) {
    //extra definitions
    $.extend($.inputmask.defaults.definitions, {
        'A': {
            validator: "[A-Za-z]",
            cardinality: 1,
            casing: "upper" //auto uppercasing
        },
        '#': {
            validator: "[A-Za-z\u0410-\u044F\u0401\u04510-9]",
            cardinality: 1,
            casing: "upper"
        }
    });
    $.extend($.inputmask.defaults.aliases, {
        'url': {
            mask: "ir",
            placeholder: "",
            separator: "",
            defaultPrefix: "http://",
            regex: {
                urlpre1: new RegExp("[fh]"),
                urlpre2: new RegExp("(ft|ht)"),
                urlpre3: new RegExp("(ftp|htt)"),
                urlpre4: new RegExp("(ftp:|http|ftps)"),
                urlpre5: new RegExp("(ftp:/|ftps:|http:|https)"),
                urlpre6: new RegExp("(ftp://|ftps:/|http:/|https:)"),
                urlpre7: new RegExp("(ftp://|ftps://|http://|https:/)"),
                urlpre8: new RegExp("(ftp://|ftps://|http://|https://)")
            },
            definitions: {
                'i': {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        return true;
                    },
                    cardinality: 8,
                    prevalidator: (function () {
                        var result = [], prefixLimit = 8;
                        for (var i = 0; i < prefixLimit; i++) {
                            result[i] = (function () {
                                var j = i;
                                return {
                                    validator: function (chrs, buffer, pos, strict, opts) {
                                        if (opts.regex["urlpre" + (j + 1)]) {
                                            var tmp = chrs, k;
                                            if (((j + 1) - chrs.length) > 0) {
                                                tmp = buffer.join('').substring(0, ((j + 1) - chrs.length)) + "" + tmp;
                                            }
                                            var isValid = opts.regex["urlpre" + (j + 1)].test(tmp);
                                            if (!strict && !isValid) {
                                                pos = pos - j;
                                                for (k = 0; k < opts.defaultPrefix.length; k++) {
                                                    buffer[pos] = opts.defaultPrefix[k]; pos++;
                                                }
                                                for (k = 0; k < tmp.length - 1; k++) {
                                                    buffer[pos] = tmp[k]; pos++;
                                                }
                                                return { "pos": pos };
                                            }
                                            return isValid;
                                        } else {
                                            return false;
                                        }
                                    }, cardinality: j
                                };
                            })();
                        }
                        return result;
                    })()
                },
                "r": {
                    validator: ".",
                    cardinality: 50
                }
            },
            insertMode: false,
            autoUnmask: false
        },
        "ip": { //ip-address mask
            mask: ["[[x]y]z.[[x]y]z.[[x]y]z.x[yz]", "[[x]y]z.[[x]y]z.[[x]y]z.[[x]y][z]"],
            definitions: {
                'x': {
                    validator: "[012]",
                    cardinality: 1,
                    definitionSymbol: "i"
                },
                'y': {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        if (pos - 1 > -1 && buffer[pos - 1] != ".")
                            chrs = buffer[pos - 1] + chrs;
                        else chrs = "0" + chrs;
                        return new RegExp("2[0-5]|[01][0-9]").test(chrs);
                    },
                    cardinality: 1,
                    definitionSymbol: "i"
                },
                'z': {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        if (pos - 1 > -1 && buffer[pos - 1] != ".") {
                            chrs = buffer[pos - 1] + chrs;
                            if (pos - 2 > -1 && buffer[pos - 2] != ".") {
                                chrs = buffer[pos - 2] + chrs;
                            } else chrs = "0" + chrs;
                        } else chrs = "00" + chrs;
                        return new RegExp("25[0-5]|2[0-4][0-9]|[01][0-9][0-9]").test(chrs);
                    },
                    cardinality: 1,
                    definitionSymbol: "i"
                }
            }
        }
    });
})(jQuery);
/*
Input Mask plugin extensions
http://github.com/RobinHerbots/jquery.inputmask
Copyright (c) 2010 - 2014 Robin Herbots
Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
Version: 2.5.0

Optional extensions on the jquery.inputmask base
*/
(function ($) {
    //date & time aliases
    $.extend($.inputmask.defaults.definitions, {
        'h': { //hours
            validator: "[01][0-9]|2[0-3]",
            cardinality: 2,
            prevalidator: [{ validator: "[0-2]", cardinality: 1}]
        },
        's': { //seconds || minutes
            validator: "[0-5][0-9]",
            cardinality: 2,
            prevalidator: [{ validator: "[0-5]", cardinality: 1}]
        },
        'd': { //basic day
            validator: "0[1-9]|[12][0-9]|3[01]",
            cardinality: 2,
            prevalidator: [{ validator: "[0-3]", cardinality: 1}]
        },
        'm': { //basic month
            validator: "0[1-9]|1[012]",
            cardinality: 2,
            prevalidator: [{ validator: "[01]", cardinality: 1}]
        },
        'y': { //basic year
            validator: "(19|20)\\d{2}",
            cardinality: 4,
            prevalidator: [
                        { validator: "[12]", cardinality: 1 },
                        { validator: "(19|20)", cardinality: 2 },
                        { validator: "(19|20)\\d", cardinality: 3 }
            ]
        }
    });
    $.extend($.inputmask.defaults.aliases, {
        'dd/mm/yyyy': {
            mask: "1/2/y",
            placeholder: "dd/mm/yyyy",
            regex: {
                val1pre: new RegExp("[0-3]"), //daypre
                val1: new RegExp("0[1-9]|[12][0-9]|3[01]"), //day
                val2pre: function (separator) { var escapedSeparator = $.inputmask.escapeRegex.call(this, separator); return new RegExp("((0[1-9]|[12][0-9]|3[01])" + escapedSeparator + "[01])"); }, //monthpre
                val2: function (separator) { var escapedSeparator = $.inputmask.escapeRegex.call(this, separator); return new RegExp("((0[1-9]|[12][0-9])" + escapedSeparator + "(0[1-9]|1[012]))|(30" + escapedSeparator + "(0[13-9]|1[012]))|(31" + escapedSeparator + "(0[13578]|1[02]))"); } //month
            },
            leapday: "29/02/",
            separator: '/',
            yearrange: { minyear: 1900, maxyear: 2099 },
            isInYearRange: function (chrs, minyear, maxyear) {
                var enteredyear = parseInt(chrs.concat(minyear.toString().slice(chrs.length)));
                var enteredyear2 = parseInt(chrs.concat(maxyear.toString().slice(chrs.length)));
                return (enteredyear != NaN ? minyear <= enteredyear && enteredyear <= maxyear : false) ||
            		   (enteredyear2 != NaN ? minyear <= enteredyear2 && enteredyear2 <= maxyear : false);
            },
            determinebaseyear: function (minyear, maxyear, hint) {
                var currentyear = (new Date()).getFullYear();
                if (minyear > currentyear) return minyear;
                if (maxyear < currentyear) {
                    var maxYearPrefix = maxyear.toString().slice(0, 2);
                    var maxYearPostfix = maxyear.toString().slice(2, 4);
                    while (maxyear < maxYearPrefix + hint) {
                        maxYearPrefix--;
                    }
                    var maxxYear = maxYearPrefix + maxYearPostfix;
                    return minyear > maxxYear ? minyear : maxxYear;
                }

                return currentyear;
            },
            onKeyUp: function (e, buffer, opts) {
                var $input = $(this);
                if (e.ctrlKey && e.keyCode == opts.keyCode.RIGHT) {
                    var today = new Date();
                    $input.val(today.getDate().toString() + (today.getMonth() + 1).toString() + today.getFullYear().toString());
                }

                //console.log($input, e, buffer, opts);
            },
            definitions: {
                '1': { //val1 ~ day or month
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var isValid = opts.regex.val1.test(chrs);
                        if (!strict && !isValid) {
                            if (chrs.charAt(1) == opts.separator || "-./".indexOf(chrs.charAt(1)) != -1) {
                                isValid = opts.regex.val1.test("0" + chrs.charAt(0));
                                if (isValid) {
                                    buffer[pos - 1] = "0";
                                    return { "pos": pos, "c": chrs.charAt(0) };
                                }
                            }
                        }
                        return isValid;
                    },
                    cardinality: 2,
                    prevalidator: [{
                        validator: function (chrs, buffer, pos, strict, opts) {
                            var isValid = opts.regex.val1pre.test(chrs);
                            if (!strict && !isValid) {
                                isValid = opts.regex.val1.test("0" + chrs);
                                if (isValid) {
                                    buffer[pos] = "0";
                                    pos++;
                                    return { "pos": pos };
                                }
                            }
                            return isValid;
                        }, cardinality: 1
                    }]
                },
                '2': { //val2 ~ day or month
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var frontValue = buffer.join('').substr(0, 3);
                        if (frontValue.indexOf(opts.placeholder[0]) != -1) frontValue = "01" + opts.separator;
                        var isValid = opts.regex.val2(opts.separator).test(frontValue + chrs);
                        if (!strict && !isValid) {
                            if (chrs.charAt(1) == opts.separator || "-./".indexOf(chrs.charAt(1)) != -1) {
                                isValid = opts.regex.val2(opts.separator).test(frontValue + "0" + chrs.charAt(0));
                                if (isValid) {
                                    buffer[pos - 1] = "0";
                                    return { "pos": pos, "c": chrs.charAt(0) };
                                }
                            }
                        }
                        return isValid;
                    },
                    cardinality: 2,
                    prevalidator: [{
                        validator: function (chrs, buffer, pos, strict, opts) {
                            var frontValue = buffer.join('').substr(0, 3);
                            if (frontValue.indexOf(opts.placeholder[0]) != -1) frontValue = "01" + opts.separator;
                            var isValid = opts.regex.val2pre(opts.separator).test(frontValue + chrs);
                            if (!strict && !isValid) {
                                isValid = opts.regex.val2(opts.separator).test(frontValue + "0" + chrs);
                                if (isValid) {
                                    buffer[pos] = "0";
                                    pos++;
                                    return { "pos": pos };
                                }
                            }
                            return isValid;
                        }, cardinality: 1
                    }]
                },
                'y': { //year
                    validator: function (chrs, buffer, pos, strict, opts) {
                        if (opts.isInYearRange(chrs, opts.yearrange.minyear, opts.yearrange.maxyear)) {
                            var dayMonthValue = buffer.join('').substr(0, 6);
                            if (dayMonthValue != opts.leapday)
                                return true;
                            else {
                                var year = parseInt(chrs, 10); //detect leap year
                                if (year % 4 === 0)
                                    if (year % 100 === 0)
                                        if (year % 400 === 0)
                                            return true;
                                        else return false;
                                    else return true;
                                else return false;
                            }
                        } else return false;
                    },
                    cardinality: 4,
                    prevalidator: [
                {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var isValid = opts.isInYearRange(chrs, opts.yearrange.minyear, opts.yearrange.maxyear);
                        if (!strict && !isValid) {
                            var yearPrefix = opts.determinebaseyear(opts.yearrange.minyear, opts.yearrange.maxyear, chrs + "0").toString().slice(0, 1);

                            isValid = opts.isInYearRange(yearPrefix + chrs, opts.yearrange.minyear, opts.yearrange.maxyear);
                            if (isValid) {
                                buffer[pos++] = yearPrefix[0];
                                return { "pos": pos };
                            }
                            yearPrefix = opts.determinebaseyear(opts.yearrange.minyear, opts.yearrange.maxyear, chrs + "0").toString().slice(0, 2);

                            isValid = opts.isInYearRange(yearPrefix + chrs, opts.yearrange.minyear, opts.yearrange.maxyear);
                            if (isValid) {
                                buffer[pos++] = yearPrefix[0];
                                buffer[pos++] = yearPrefix[1];
                                return { "pos": pos };
                            }
                        }
                        return isValid;
                    },
                    cardinality: 1
                },
                {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var isValid = opts.isInYearRange(chrs, opts.yearrange.minyear, opts.yearrange.maxyear);
                        if (!strict && !isValid) {
                            var yearPrefix = opts.determinebaseyear(opts.yearrange.minyear, opts.yearrange.maxyear, chrs).toString().slice(0, 2);

                            isValid = opts.isInYearRange(chrs[0] + yearPrefix[1] + chrs[1], opts.yearrange.minyear, opts.yearrange.maxyear);
                            if (isValid) {
                                buffer[pos++] = yearPrefix[1];
                                return { "pos": pos };
                            }

                            yearPrefix = opts.determinebaseyear(opts.yearrange.minyear, opts.yearrange.maxyear, chrs).toString().slice(0, 2);
                            if (opts.isInYearRange(yearPrefix + chrs, opts.yearrange.minyear, opts.yearrange.maxyear)) {
                                var dayMonthValue = buffer.join('').substr(0, 6);
                                if (dayMonthValue != opts.leapday)
                                    isValid = true;
                                else {
                                    var year = parseInt(chrs, 10); //detect leap year
                                    if (year % 4 === 0)
                                        if (year % 100 === 0)
                                            if (year % 400 === 0)
                                                isValid = true;
                                            else isValid = false;
                                        else isValid = true;
                                    else isValid = false;
                                }
                            } else isValid = false;
                            if (isValid) {
                                buffer[pos - 1] = yearPrefix[0];
                                buffer[pos++] = yearPrefix[1];
                                buffer[pos++] = chrs[0];
                                return { "pos": pos };
                            }
                        }
                        return isValid;
                    }, cardinality: 2
                },
                {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        return opts.isInYearRange(chrs, opts.yearrange.minyear, opts.yearrange.maxyear);
                    }, cardinality: 3
                }
                    ]
                }
            },
            insertMode: false,
            autoUnmask: false
        },
        'mm/dd/yyyy': {
            placeholder: "mm/dd/yyyy",
            alias: "dd/mm/yyyy", //reuse functionality of dd/mm/yyyy alias
            regex: {
                val2pre: function (separator) { var escapedSeparator = $.inputmask.escapeRegex.call(this, separator); return new RegExp("((0[13-9]|1[012])" + escapedSeparator + "[0-3])|(02" + escapedSeparator + "[0-2])"); }, //daypre
                val2: function (separator) { var escapedSeparator = $.inputmask.escapeRegex.call(this, separator); return new RegExp("((0[1-9]|1[012])" + escapedSeparator + "(0[1-9]|[12][0-9]))|((0[13-9]|1[012])" + escapedSeparator + "30)|((0[13578]|1[02])" + escapedSeparator + "31)"); }, //day
                val1pre: new RegExp("[01]"), //monthpre
                val1: new RegExp("0[1-9]|1[012]") //month
            },
            leapday: "02/29/",
            onKeyUp: function (e, buffer, opts) {
                var $input = $(this);
                if (e.ctrlKey && e.keyCode == opts.keyCode.RIGHT) {
                    var today = new Date();
                    $input.val((today.getMonth() + 1).toString() + today.getDate().toString() + today.getFullYear().toString());
                }
            }
        },
        'yyyy/mm/dd': {
            mask: "y/1/2",
            placeholder: "yyyy/mm/dd",
            alias: "mm/dd/yyyy",
            leapday: "/02/29",
            onKeyUp: function (e, buffer, opts) {
                var $input = $(this);
                if (e.ctrlKey && e.keyCode == opts.keyCode.RIGHT) {
                    var today = new Date();
                    $input.val(today.getFullYear().toString() + (today.getMonth() + 1).toString() + today.getDate().toString());
                }
            },
            definitions: {
                '2': { //val2 ~ day or month
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var frontValue = buffer.join('').substr(5, 3);
                        if (frontValue.indexOf(opts.placeholder[5]) != -1) frontValue = "01" + opts.separator;
                        var isValid = opts.regex.val2(opts.separator).test(frontValue + chrs);
                        if (!strict && !isValid) {
                            if (chrs.charAt(1) == opts.separator || "-./".indexOf(chrs.charAt(1)) != -1) {
                                isValid = opts.regex.val2(opts.separator).test(frontValue + "0" + chrs.charAt(0));
                                if (isValid) {
                                    buffer[pos - 1] = "0";
                                    return { "pos": pos, "c": chrs.charAt(0) };
                                }
                            }
                        }

                        //check leap yeap
                        if (isValid) {
                            var dayMonthValue = buffer.join('').substr(4, 4) + chrs;
                            if (dayMonthValue != opts.leapday)
                                return true;
                            else {
                                var year = parseInt(buffer.join('').substr(0, 4), 10);  //detect leap year
                                if (year % 4 === 0)
                                    if (year % 100 === 0)
                                        if (year % 400 === 0)
                                            return true;
                                        else return false;
                                    else return true;
                                else return false;
                            }
                        }

                        return isValid;
                    },
                    cardinality: 2,
                    prevalidator: [{
                        validator: function (chrs, buffer, pos, strict, opts) {
                            var frontValue = buffer.join('').substr(5, 3);
                            if (frontValue.indexOf(opts.placeholder[5]) != -1) frontValue = "01" + opts.separator;
                            var isValid = opts.regex.val2pre(opts.separator).test(frontValue + chrs);
                            if (!strict && !isValid) {
                                isValid = opts.regex.val2(opts.separator).test(frontValue + "0" + chrs);
                                if (isValid) {
                                    buffer[pos] = "0";
                                    pos++;
                                    return { "pos": pos };
                                }
                            }
                            return isValid;
                        }, cardinality: 1
                    }]
                }
            }
        },
        'dd.mm.yyyy': {
            mask: "1.2.y",
            placeholder: "dd.mm.yyyy",
            leapday: "29.02.",
            separator: '.',
            alias: "dd/mm/yyyy"
        },
        'dd-mm-yyyy': {
            mask: "1-2-y",
            placeholder: "__-__-____",
            //showMaskOnHover: false,
            leapday: "29-02-",
            separator: '-',
            alias: "dd/mm/yyyy"
        },
        'mm.dd.yyyy': {
            mask: "1.2.y",
            placeholder: "mm.dd.yyyy",
            leapday: "02.29.",
            separator: '.',
            alias: "mm/dd/yyyy"
        },
        'mm-dd-yyyy': {
            mask: "1-2-y",
            placeholder: "mm-dd-yyyy",
            leapday: "02-29-",
            separator: '-',
            alias: "mm/dd/yyyy"
        },
        'yyyy.mm.dd': {
            mask: "y.1.2",
            placeholder: "yyyy.mm.dd",
            leapday: ".02.29",
            separator: '.',
            alias: "yyyy/mm/dd"
        },
        'yyyy-mm-dd': {
            mask: "y-1-2",
            placeholder: "yyyy-mm-dd",
            leapday: "-02-29",
            separator: '-',
            alias: "yyyy/mm/dd"
        },
        'datetime': {
            mask: "1/2/y h:s",
            placeholder: "dd/mm/yyyy hh:mm",
            alias: "dd/mm/yyyy",
            regex: {
                hrspre: new RegExp("[012]"), //hours pre
                hrs24: new RegExp("2[0-9]|1[3-9]"),
                hrs: new RegExp("[01][0-9]|2[0-3]"), //hours
                ampm: new RegExp("^[a|p|A|P][m|M]")
            },
            timeseparator: ':',
            hourFormat: "24", // or 12
            definitions: {
                'h': { //hours
                    validator: function (chrs, buffer, pos, strict, opts) {
                        var isValid = opts.regex.hrs.test(chrs);
                        if (!strict && !isValid) {
                            if (chrs.charAt(1) == opts.timeseparator || "-.:".indexOf(chrs.charAt(1)) != -1) {
                                isValid = opts.regex.hrs.test("0" + chrs.charAt(0));
                                if (isValid) {
                                    buffer[pos - 1] = "0";
                                    buffer[pos] = chrs.charAt(0);
                                    pos++;
                                    return { "pos": pos };
                                }
                            }
                        }

                        if (isValid && opts.hourFormat !== "24" && opts.regex.hrs24.test(chrs)) {

                            var tmp = parseInt(chrs, 10);

                            if (tmp == 24) {
                                buffer[pos + 5] = "a";
                                buffer[pos + 6] = "m";
                            } else {
                                buffer[pos + 5] = "p";
                                buffer[pos + 6] = "m";
                            }

                            tmp = tmp - 12;

                            if (tmp < 10) {
                                buffer[pos] = tmp.toString();
                                buffer[pos - 1] = "0";
                            } else {
                                buffer[pos] = tmp.toString().charAt(1);
                                buffer[pos - 1] = tmp.toString().charAt(0);
                            }

                            return { "pos": pos, "c": buffer[pos] };
                        }

                        return isValid;
                    },
                    cardinality: 2,
                    prevalidator: [{
                        validator: function (chrs, buffer, pos, strict, opts) {
                            var isValid = opts.regex.hrspre.test(chrs);
                            if (!strict && !isValid) {
                                isValid = opts.regex.hrs.test("0" + chrs);
                                if (isValid) {
                                    buffer[pos] = "0";
                                    pos++;
                                    return { "pos": pos };
                                }
                            }
                            return isValid;
                        }, cardinality: 1
                    }]
                },
                't': { //am/pm
                    validator: function (chrs, buffer, pos, strict, opts) {
                        return opts.regex.ampm.test(chrs + "m");
                    },
                    casing: "lower",
                    cardinality: 1
                }
            },
            insertMode: false,
            autoUnmask: false
        },
        'datetime12': {
            mask: "1/2/y h:s t\\m",
            placeholder: "dd/mm/yyyy hh:mm xm",
            alias: "datetime",
            hourFormat: "12"
        },
        'hh:mm t': {
            mask: "h:s t\\m",
            placeholder: "hh:mm xm",
            alias: "datetime",
            hourFormat: "12"
        },
        'h:s t': {
            mask: "h:s t\\m",
            placeholder: "hh:mm xm",
            alias: "datetime",
            hourFormat: "12"
        },
        'hh:mm:ss': {
            mask: "h:s:s",
            autoUnmask: false
        },
        'hh:mm': {
            mask: "h:s",
            autoUnmask: false
        },
        'date': {
            alias: "dd-mm-yyyy" // "mm/dd/yyyy"
        },
        'mm/yyyy': {
            mask: "1/y",
            placeholder: "mm/yyyy",
            leapday: "donotuse",
            separator: '/',
            alias: "mm/dd/yyyy"
        }
    });
})(jQuery);
/*
Input Mask plugin extensions
http://github.com/RobinHerbots/jquery.inputmask
Copyright (c) 2010 - 2014 Robin Herbots
Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
Version: 2.5.0

Optional extensions on the jquery.inputmask base
*/
(function ($) {
    //number aliases
    $.extend($.inputmask.defaults.aliases, {
        'decimal': {
            mask: "~",
            placeholder: "",
            repeat: "*",
            greedy: false,
            numericInput: false,
            isNumeric: true,
            digits: "*", //number of fractionalDigits
            groupSeparator: "", //",", // | "."
            radixPoint: ".",
            groupSize: 3,
            autoGroup: false,
            allowPlus: true,
            allowMinus: true,
            //todo
            integerDigits: "*", //number of integerDigits
            defaultValue: "",
            prefix: "",
            suffix: "",

            //todo
            getMaskLength: function (buffer, greedy, repeat, currentBuffer, opts) { //custom getMaskLength to take the groupSeparator into account
                var calculatedLength = buffer.length;

                if (!greedy) {
                    if (repeat == "*") {
                        calculatedLength = currentBuffer.length + 1;
                    } else if (repeat > 1) {
                        calculatedLength += (buffer.length * (repeat - 1));
                    }
                }

                var escapedGroupSeparator = $.inputmask.escapeRegex.call(this, opts.groupSeparator);
                var escapedRadixPoint = $.inputmask.escapeRegex.call(this, opts.radixPoint);
                var currentBufferStr = currentBuffer.join(''), strippedBufferStr = currentBufferStr.replace(new RegExp(escapedGroupSeparator, "g"), "").replace(new RegExp(escapedRadixPoint), ""),
                groupOffset = currentBufferStr.length - strippedBufferStr.length;
                return calculatedLength + groupOffset;
            },
            postFormat: function (buffer, pos, reformatOnly, opts) {
                if (opts.groupSeparator == "") return pos;
                var cbuf = buffer.slice(),
                    radixPos = $.inArray(opts.radixPoint, buffer);
                if (!reformatOnly) {
                    cbuf.splice(pos, 0, "?"); //set position indicator
                }
                var bufVal = cbuf.join('');
                if (opts.autoGroup || (reformatOnly && bufVal.indexOf(opts.groupSeparator) != -1)) {
                    var escapedGroupSeparator = $.inputmask.escapeRegex.call(this, opts.groupSeparator);
                    bufVal = bufVal.replace(new RegExp(escapedGroupSeparator, "g"), '');
                    var radixSplit = bufVal.split(opts.radixPoint);
                    bufVal = radixSplit[0];
                    var reg = new RegExp('([-\+]?[\\d\?]+)([\\d\?]{' + opts.groupSize + '})');
                    while (reg.test(bufVal)) {
                        bufVal = bufVal.replace(reg, '$1' + opts.groupSeparator + '$2');
                        bufVal = bufVal.replace(opts.groupSeparator + opts.groupSeparator, opts.groupSeparator);
                    }
                    if (radixSplit.length > 1)
                        bufVal += opts.radixPoint + radixSplit[1];
                }
                buffer.length = bufVal.length; //align the length
                for (var i = 0, l = bufVal.length; i < l; i++) {
                    buffer[i] = bufVal.charAt(i);
                }
                var newPos = $.inArray("?", buffer);
                if (!reformatOnly) buffer.splice(newPos, 1);

                return reformatOnly ? pos : newPos;
            },
            regex: {
                number: function (opts) {
                    var escapedGroupSeparator = $.inputmask.escapeRegex.call(this, opts.groupSeparator);
                    var escapedRadixPoint = $.inputmask.escapeRegex.call(this, opts.radixPoint);
                    var digitExpression = isNaN(opts.digits) ? opts.digits : '{0,' + opts.digits + '}';
                    var signedExpression = opts.allowPlus || opts.allowMinus ? "[" + (opts.allowPlus ? "\+" : "") + (opts.allowMinus ? "-" : "") + "]?" : "";
                    return new RegExp("^" + signedExpression + "(\\d+|\\d{1," + opts.groupSize + "}((" + escapedGroupSeparator + "\\d{" + opts.groupSize + "})?)+)(" + escapedRadixPoint + "\\d" + digitExpression + ")?$");
                }
            },
            onKeyDown: function (e, buffer, opts) {
                var $input = $(this), input = this;
                if (e.keyCode == opts.keyCode.TAB) {
                    var radixPosition = $.inArray(opts.radixPoint, buffer);
                    if (radixPosition != -1) {
                        var masksets = $input.data('_inputmask')['masksets'];
                        var activeMasksetIndex = $input.data('_inputmask')['activeMasksetIndex'];
                        for (var i = 1; i <= opts.digits && i < opts.getMaskLength(masksets[activeMasksetIndex]["_buffer"], masksets[activeMasksetIndex]["greedy"], masksets[activeMasksetIndex]["repeat"], buffer, opts); i++) {
                            if (buffer[radixPosition + i] == undefined || buffer[radixPosition + i] == "") buffer[radixPosition + i] = "0";
                        }
                        input._valueSet(buffer.join(''));
                    }
                } else if (e.keyCode == opts.keyCode.DELETE || e.keyCode == opts.keyCode.BACKSPACE) {
                    opts.postFormat(buffer, 0, true, opts);
                    input._valueSet(buffer.join(''));
                    return true;
                }
            },
            definitions: {
                '~': { //real number
                    validator: function (chrs, buffer, pos, strict, opts) {
                        if (chrs == "") return false;
                        if (!strict && pos <= 1 && buffer[0] === '0' && new RegExp("[\\d-]").test(chrs) && buffer.join('').length == 1) { //handle first char
                            buffer[0] = "";
                            return { "pos": 0 };
                        }

                        var cbuf = strict ? buffer.slice(0, pos) : buffer.slice();

                        cbuf.splice(pos, 0, chrs);
                        var bufferStr = cbuf.join('');

                        //strip groupseparator
                        var escapedGroupSeparator = $.inputmask.escapeRegex.call(this, opts.groupSeparator);
                        bufferStr = bufferStr.replace(new RegExp(escapedGroupSeparator, "g"), '');

                        var isValid = opts.regex.number(opts).test(bufferStr);
                        if (!isValid) {
                            //let's help the regex a bit
                            bufferStr += "0";
                            isValid = opts.regex.number(opts).test(bufferStr);
                            if (!isValid) {
                                //make a valid group
                                var lastGroupSeparator = bufferStr.lastIndexOf(opts.groupSeparator);
                                for (var i = bufferStr.length - lastGroupSeparator; i <= 3; i++) {
                                    bufferStr += "0";
                                }

                                isValid = opts.regex.number(opts).test(bufferStr);
                                if (!isValid && !strict) {
                                    if (chrs == opts.radixPoint) {
                                        isValid = opts.regex.number(opts).test("0" + bufferStr + "0");
                                        if (isValid) {
                                            buffer[pos] = "0";
                                            pos++;
                                            return { "pos": pos };
                                        }
                                    }
                                }
                            }
                        }

                        if (isValid != false && !strict && chrs != opts.radixPoint) {
                            var newPos = opts.postFormat(buffer, pos, false, opts);
                            return { "pos": newPos };
                        }

                        return isValid;
                    },
                    cardinality: 1,
                    prevalidator: null
                }
            },
            insertMode: true,
            autoUnmask: false
        },
        'integer': {
            regex: {
                number: function (opts) {
                    var escapedGroupSeparator = $.inputmask.escapeRegex.call(this, opts.groupSeparator);
                    var signedExpression = opts.allowPlus || opts.allowMinus ? "[" + (opts.allowPlus ? "\+" : "") + (opts.allowMinus ? "-" : "") + "]?" : "";
                    return new RegExp("^" + signedExpression + "(\\d+|\\d{1," + opts.groupSize + "}((" + escapedGroupSeparator + "\\d{" + opts.groupSize + "})?)+)$");
                }
            },
            alias: "decimal"
        }
    });
})(jQuery);
/*
Input Mask plugin extensions
http://github.com/RobinHerbots/jquery.inputmask
Copyright (c) 2010 - 2014 Robin Herbots
Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
Version: 2.5.0

Regex extensions on the jquery.inputmask base
Allows for using regular expressions as a mask
*/
(function ($) {
    $.extend($.inputmask.defaults.aliases, { // $(selector).inputmask("Regex", { regex: "[0-9]*"}
        'Regex': {
            mask: "r",
            greedy: false,
            repeat: "*",
            regex: null,
            regexTokens: null,
            //Thx to https://github.com/slevithan/regex-colorizer for the tokenizer regex
            tokenizer: /\[\^?]?(?:[^\\\]]+|\\[\S\s]?)*]?|\\(?:0(?:[0-3][0-7]{0,2}|[4-7][0-7]?)?|[1-9][0-9]*|x[0-9A-Fa-f]{2}|u[0-9A-Fa-f]{4}|c[A-Za-z]|[\S\s]?)|\((?:\?[:=!]?)?|(?:[?*+]|\{[0-9]+(?:,[0-9]*)?\})\??|[^.?*+^${[()|\\]+|./g,
            quantifierFilter: /[0-9]+[^,]/,
            definitions: {
                'r': {
                    validator: function (chrs, buffer, pos, strict, opts) {
                        function regexToken() {
                            this.matches = [];
                            this.isGroup = false;
                            this.isQuantifier = false;
                            this.isLiteral = false;
                        }
                        function analyseRegex() {
                            var currentToken = new regexToken(), match, m, opengroups = [];

                            opts.regexTokens = [];

                            // The tokenizer regex does most of the tokenization grunt work
                            while (match = opts.tokenizer.exec(opts.regex)) {
                                m = match[0];
                                switch (m.charAt(0)) {
                                    case "[": // Character class
                                    case "\\":  // Escape or backreference
                                        if (opengroups.length > 0) {
                                            opengroups[opengroups.length - 1]["matches"].push(m);
                                        } else {
                                            currentToken.matches.push(m);
                                        }
                                        break;
                                    case "(": // Group opening
                                        if (!currentToken.isGroup && currentToken.matches.length > 0)
                                            opts.regexTokens.push(currentToken);
                                        currentToken = new regexToken();
                                        currentToken.isGroup = true;
                                        opengroups.push(currentToken);
                                        break;
                                    case ")": // Group closing
                                        var groupToken = opengroups.pop();
                                        if (opengroups.length > 0) {
                                            opengroups[opengroups.length - 1]["matches"].push(groupToken);
                                        } else {
                                            opts.regexTokens.push(groupToken);
                                            currentToken = new regexToken();
                                        }
                                        break;
                                    case "{": //Quantifier
                                        var quantifier = new regexToken();
                                        quantifier.isQuantifier = true;
                                        quantifier.matches.push(m);
                                        if (opengroups.length > 0) {
                                            opengroups[opengroups.length - 1]["matches"].push(quantifier);
                                        } else {
                                            currentToken.matches.push(quantifier);
                                        }
                                        break;
                                    default:
                                        // Vertical bar (alternator) 
                                        // ^ or $ anchor
                                        // Dot (.)
                                        // Literal character sequence
                                        var literal = new regexToken();
                                        literal.isLiteral = true;
                                        literal.matches.push(m);
                                        if (opengroups.length > 0) {
                                            opengroups[opengroups.length - 1]["matches"].push(literal);
                                        } else {
                                            currentToken.matches.push(literal);
                                        }
                                }
                            }

                            if (currentToken.matches.length > 0)
                                opts.regexTokens.push(currentToken);
                        };

                        function validateRegexToken(token, fromGroup) {
                            var isvalid = false;
                            if (fromGroup) {
                                regexPart += "(";
                                openGroupCount++;
                            }
                            for (var mndx = 0; mndx < token["matches"].length; mndx++) {
                                var matchToken = token["matches"][mndx];
                                if (matchToken["isGroup"] == true) {
                                    isvalid = validateRegexToken(matchToken, true);
                                } else if (matchToken["isQuantifier"] == true) {
                                    matchToken = matchToken["matches"][0];
                                    var quantifierMax = opts.quantifierFilter.exec(matchToken)[0].replace("}", "");
                                    var testExp = regexPart + "{1," + quantifierMax + "}"; //relax quantifier validation
                                    for (var j = 0; j < openGroupCount; j++) {
                                        testExp += ")";
                                    }
                                    var exp = new RegExp("^(" + testExp + ")$");
                                    isvalid = exp.test(bufferStr);
                                    regexPart += matchToken;
                                } else if (matchToken["isLiteral"] == true) {
                                    matchToken = matchToken["matches"][0];
                                    var testExp = regexPart, openGroupCloser = "";
                                    for (var j = 0; j < openGroupCount; j++) {
                                        openGroupCloser += ")";
                                    }
                                    for (var k = 0; k < matchToken.length; k++) { //relax literal validation
                                        testExp = (testExp + matchToken[k]).replace(/\|$/, "");
                                        var exp = new RegExp("^(" + testExp + openGroupCloser + ")$");
                                        isvalid = exp.test(bufferStr);
                                        if (isvalid) break;
                                    }
                                    regexPart += matchToken;
                                    //console.log(bufferStr + " " + exp + " " + isvalid);
                                } else {
                                    regexPart += matchToken;
                                    var testExp = regexPart.replace(/\|$/, "");
                                    for (var j = 0; j < openGroupCount; j++) {
                                        testExp += ")";
                                    }
                                    var exp = new RegExp("^(" + testExp + ")$");
                                    isvalid = exp.test(bufferStr);
                                    //console.log(bufferStr + " " + exp + " " + isvalid);
                                }
                                if (isvalid) break;
                            }

                            if (fromGroup) {
                                regexPart += ")";
                                openGroupCount--;
                            }

                            return isvalid;
                        }


                        if (opts.regexTokens == null) {
                            analyseRegex();
                        }

                        var cbuffer = buffer.slice(), regexPart = "", isValid = false, openGroupCount = 0;
                        cbuffer.splice(pos, 0, chrs);
                        var bufferStr = cbuffer.join('');
                        for (var i = 0; i < opts.regexTokens.length; i++) {
                            var regexToken = opts.regexTokens[i];
                            isValid = validateRegexToken(regexToken, regexToken["isGroup"]);
                            if (isValid) break;
                        }

                        return isValid;
                    },
                    cardinality: 1
                }
            }
        }
    });
})(jQuery);
/*
Input Mask plugin extensions
http://github.com/RobinHerbots/jquery.inputmask
Copyright (c) 2010 - 2014 Robin Herbots
Licensed under the MIT license (http://www.opensource.org/licenses/mit-license.php)
Version: 2.5.0

Phone extension.
When using this extension make sure you specify the correct url to get the masks

$(selector).inputmask("phone", {
url: "Scripts/jquery.inputmask/phone-codes/phone-codes.json", 
onKeyValidation: function () { //show some metadata in the console
console.log($(this).inputmask("getmetadata")["name_en"]);
} 
});


*/
(function ($) {
    $.extend($.inputmask.defaults.aliases, {
        'phone': {
            url: "phone-codes/phone-codes.json",
            mask: function (opts) {
                opts.definitions = {
                    'p': {
                        validator: function () { return false; },
                        cardinality: 1
                    },
                    '#': {
                        validator: "[0-9]",
                        cardinality: 1
                    }
                };
                var maskList = [];
                $.ajax({
                    url: opts.url,
                    async: false,
                    dataType: 'json',
                    success: function (response) {
                        maskList = response;
                    }
                });

                maskList.splice(0, 0, "+p(ppp)ppp-pppp");
                return maskList;
            }
        }
    });
})(jQuery);


/*
 * jQuery UI Multi Open Accordion Plugin
 * Author	: Anas Nakawa (http://anasnakawa.wordpress.com/)
 * Date		: 22-Jul-2011
 * Released Under MIT License
 * You are welcome to enhance this plugin at https://code.google.com/p/jquery-multi-open-accordion/
 */
(function(a){a.widget("ui.multiOpenAccordion",{options:{active:0,showAll:null,hideAll:null,_classes:{accordion:"ui-accordion ui-widget ui-helper-reset ui-accordion-icons",h3:"ui-accordion-header ui-helper-reset ui-state-default ui-corner-all",div:"ui-accordion-content ui-helper-reset ui-widget-content ui-corner-bottom",divActive:"ui-accordion-content-active",span:"ui-icon ui-icon-triangle-1-e",stateDefault:"ui-state-default",stateHover:"ui-state-hover"}},_create:function(){var b=this,c=b.options,d=b.element,e=d.children("h3"),f=d.children("div");d.addClass(c._classes.accordion);e.each(function(d){var e=a(this);e.addClass(c._classes.h3).prepend('<span class="{class}"></span>'.replace(/{class}/,c._classes.span));if(b._isActive(d)){b._showTab(e)}});d.children("div").each(function(b){var d=a(this);d.addClass(c._classes.div)});e.bind("click",function(d){d.preventDefault();var e=a(this);var f={tab:e,content:e.next("div")};b._trigger("click",null,f);if(e.hasClass(c._classes.stateDefault)){b._showTab(e)}else{b._hideTab(e)}});e.bind("mouseover",function(){a(this).addClass(c._classes.stateHover)});e.bind("mouseout",function(){a(this).removeClass(c._classes.stateHover)});b._trigger("init",null,d)},destroy:function(){var a=this;var b=a.element;var c=b.children("h3");var d=b.children("div");var e=a.options;b.children("h3").unbind("click mouseover mouseout");b.removeClass(e._classes.accordion);c.removeClass(e._classes.h3).removeClass("ui-state-default ui-corner-all ui-state-active ui-corner-top").children("span").remove();d.removeClass(e._classes.div+" "+e._classes.divActive).show()},_showTab:function(a){var b=a.children("span.ui-icon");var c=a.next();var d=this.options;a.removeClass("ui-state-default ui-corner-all").addClass("ui-state-active ui-corner-top");b.removeClass("ui-icon-triangle-1-e").addClass("ui-icon-triangle-1-s");c.slideDown("fast",function(){c.addClass(d._classes.divActive)});var e={tab:a,content:a.next("div")};this._trigger("tabShown",null,e)},_hideTab:function(a){var b=a.children("span.ui-icon");var c=a.next();var d=this.options;a.removeClass("ui-state-active ui-corner-top").addClass("ui-state-default ui-corner-all");b.removeClass("ui-icon-triangle-1-s").addClass("ui-icon-triangle-1-e");c.slideUp("fast",function(){c.removeClass(d._classes.divActive)});var e={tab:a,content:a.next("div")};this._trigger("tabHidden",null,e)},_isActive:function(a){var b=this.options;if(typeof b.active=="boolean"&&!b.active){return false}else{if(b.active.length!=undefined){for(var c=0;c<b.active.length;c++){if(b.active[c]==a)return true}}else{return b.active==a}}return false},_getActiveTabs:function(){var b=this.element;var c=[];b.children("div").each(function(b){var d=a(this);if(d.is(":visible")){c.push({index:b,tab:d.prev("h3"),content:d})}});return c.length==0?undefined:c},getActiveTabs:function(){var b=this.element;var c=[];b.children("div").each(function(b){if(a(this).is(":visible")){c.push(b)}});return c.length==0?[-1]:c},_setActiveTabs:function(b){var c=this;var d=this.element;if(typeof b!="undefined"){d.children("div").each(function(d){var e=a(this).prev("h3");if(b.hasObject(d)){c._showTab(e)}else{c._hideTab(e)}})}},_generateTabsArrayFromOptions:function(b){var c=[];var d=this;var e=d.element;var f=e.children("h3").size();if(a.type(b)==="array"){return b}else if(a.type(b)==="number"){return[b]}else if(a.type(b)==="string"){switch(b.toLowerCase()){case"all":var f=e.children("h3").size();for(var g=0;g<f;g++){c.push(g)}return c;break;case"none":c=[-1];return c;break;default:return undefined;break}}},_setOption:function(b,c){a.Widget.prototype._setOption.apply(this,arguments);var d=this.element;switch(b){case"active":this._setActiveTabs(this._generateTabsArrayFromOptions(c));break;case"getActiveTabs":var d=this.element;var e;d.children("div").each(function(b){if(a(this).is(":visible")){e=e?e:[];e.push(b)}});return e.length==0?[-1]:e;break}}});Array.prototype.hasObject=!Array.indexOf?function(a){var b=this.length+1;while(b-=1){if(this[b-1]===a){return true}}return false}:function(a){return this.indexOf(a)!==-1}})(jQuery);


/**
 * Version: 1.0 Alpha-1 
 * Build Date: 13-Nov-2007
 * Copyright (c) 2006-2007, Coolite Inc. (http://www.coolite.com/). All rights reserved.
 * License: Licensed under The MIT License. See license.txt and http://www.datejs.com/license/. 
 * Website: http://www.datejs.com/ or http://www.coolite.com/datejs/
 */
Date.CultureInfo = { name: "en-US", englishName: "English (United States)", nativeName: "English (United States)", dayNames: ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"], abbreviatedDayNames: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"], shortestDayNames: ["Su", "Mo", "Tu", "We", "Th", "Fr", "Sa"], firstLetterDayNames: ["S", "M", "T", "W", "T", "F", "S"], monthNames: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], abbreviatedMonthNames: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"], amDesignator: "AM", pmDesignator: "PM", firstDayOfWeek: 0, twoDigitYearMax: 2029, dateElementOrder: "mdy", formatPatterns: { shortDate: "M/d/yyyy", longDate: "dddd, MMMM dd, yyyy", shortTime: "h:mm tt", longTime: "h:mm:ss tt", fullDateTime: "dddd, MMMM dd, yyyy h:mm:ss tt", sortableDateTime: "yyyy-MM-ddTHH:mm:ss", universalSortableDateTime: "yyyy-MM-dd HH:mm:ssZ", rfc1123: "ddd, dd MMM yyyy HH:mm:ss GMT", monthDay: "MMMM dd", yearMonth: "MMMM, yyyy" }, regexPatterns: { jan: /^jan(uary)?/i, feb: /^feb(ruary)?/i, mar: /^mar(ch)?/i, apr: /^apr(il)?/i, may: /^may/i, jun: /^jun(e)?/i, jul: /^jul(y)?/i, aug: /^aug(ust)?/i, sep: /^sep(t(ember)?)?/i, oct: /^oct(ober)?/i, nov: /^nov(ember)?/i, dec: /^dec(ember)?/i, sun: /^su(n(day)?)?/i, mon: /^mo(n(day)?)?/i, tue: /^tu(e(s(day)?)?)?/i, wed: /^we(d(nesday)?)?/i, thu: /^th(u(r(s(day)?)?)?)?/i, fri: /^fr(i(day)?)?/i, sat: /^sa(t(urday)?)?/i, future: /^next/i, past: /^last|past|prev(ious)?/i, add: /^(\+|after|from)/i, subtract: /^(\-|before|ago)/i, yesterday: /^yesterday/i, today: /^t(oday)?/i, tomorrow: /^tomorrow/i, now: /^n(ow)?/i, millisecond: /^ms|milli(second)?s?/i, second: /^sec(ond)?s?/i, minute: /^min(ute)?s?/i, hour: /^h(ou)?rs?/i, week: /^w(ee)?k/i, month: /^m(o(nth)?s?)?/i, day: /^d(ays?)?/i, year: /^y((ea)?rs?)?/i, shortMeridian: /^(a|p)/i, longMeridian: /^(a\.?m?\.?|p\.?m?\.?)/i, timezone: /^((e(s|d)t|c(s|d)t|m(s|d)t|p(s|d)t)|((gmt)?\s*(\+|\-)\s*\d\d\d\d?)|gmt)/i, ordinalSuffix: /^\s*(st|nd|rd|th)/i, timeContext: /^\s*(\:|a|p)/i }, abbreviatedTimeZoneStandard: { GMT: "-000", EST: "-0400", CST: "-0500", MST: "-0600", PST: "-0700" }, abbreviatedTimeZoneDST: { GMT: "-000", EDT: "-0500", CDT: "-0600", MDT: "-0700", PDT: "-0800" } };
Date.getMonthNumberFromName = function (name) {
    var n = Date.CultureInfo.monthNames, m = Date.CultureInfo.abbreviatedMonthNames, s = name.toLowerCase(); for (var i = 0; i < n.length; i++) { if (n[i].toLowerCase() == s || m[i].toLowerCase() == s) { return i; } }
    return -1;
}; Date.getDayNumberFromName = function (name) {
    var n = Date.CultureInfo.dayNames, m = Date.CultureInfo.abbreviatedDayNames, o = Date.CultureInfo.shortestDayNames, s = name.toLowerCase(); for (var i = 0; i < n.length; i++) { if (n[i].toLowerCase() == s || m[i].toLowerCase() == s) { return i; } }
    return -1;
}; Date.isLeapYear = function (year) { return (((year % 4 === 0) && (year % 100 !== 0)) || (year % 400 === 0)); }; Date.getDaysInMonth = function (year, month) { return [31, (Date.isLeapYear(year) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month]; }; Date.getTimezoneOffset = function (s, dst) { return (dst || false) ? Date.CultureInfo.abbreviatedTimeZoneDST[s.toUpperCase()] : Date.CultureInfo.abbreviatedTimeZoneStandard[s.toUpperCase()]; }; Date.getTimezoneAbbreviation = function (offset, dst) {
    var n = (dst || false) ? Date.CultureInfo.abbreviatedTimeZoneDST : Date.CultureInfo.abbreviatedTimeZoneStandard, p; for (p in n) { if (n[p] === offset) { return p; } }
    return null;
}; Date.prototype.clone = function () { return new Date(this.getTime()); }; Date.prototype.compareTo = function (date) {
    if (isNaN(this)) { throw new Error(this); }
    if (date instanceof Date && !isNaN(date)) { return (this > date) ? 1 : (this < date) ? -1 : 0; } else { throw new TypeError(date); }
}; Date.prototype.equals = function (date) { return (this.compareTo(date) === 0); }; Date.prototype.between = function (start, end) { var t = this.getTime(); return t >= start.getTime() && t <= end.getTime(); }; Date.prototype.addMilliseconds = function (value) { this.setMilliseconds(this.getMilliseconds() + value); return this; }; Date.prototype.addSeconds = function (value) { return this.addMilliseconds(value * 1000); }; Date.prototype.addMinutes = function (value) { return this.addMilliseconds(value * 60000); }; Date.prototype.addHours = function (value) { return this.addMilliseconds(value * 3600000); }; Date.prototype.addDays = function (value) { return this.addMilliseconds(value * 86400000); }; Date.prototype.addWeeks = function (value) { return this.addMilliseconds(value * 604800000); }; Date.prototype.addMonths = function (value) { var n = this.getDate(); this.setDate(1); this.setMonth(this.getMonth() + value); this.setDate(Math.min(n, this.getDaysInMonth())); return this; }; Date.prototype.addYears = function (value) { return this.addMonths(value * 12); }; Date.prototype.add = function (config) {
    if (typeof config == "number") { this._orient = config; return this; }
    var x = config; if (x.millisecond || x.milliseconds) { this.addMilliseconds(x.millisecond || x.milliseconds); }
    if (x.second || x.seconds) { this.addSeconds(x.second || x.seconds); }
    if (x.minute || x.minutes) { this.addMinutes(x.minute || x.minutes); }
    if (x.hour || x.hours) { this.addHours(x.hour || x.hours); }
    if (x.month || x.months) { this.addMonths(x.month || x.months); }
    if (x.year || x.years) { this.addYears(x.year || x.years); }
    if (x.day || x.days) { this.addDays(x.day || x.days); }
    return this;
}; Date._validate = function (value, min, max, name) {
    if (typeof value != "number") { throw new TypeError(value + " is not a Number."); } else if (value < min || value > max) { throw new RangeError(value + " is not a valid value for " + name + "."); }
    return true;
}; Date.validateMillisecond = function (n) { return Date._validate(n, 0, 999, "milliseconds"); }; Date.validateSecond = function (n) { return Date._validate(n, 0, 59, "seconds"); }; Date.validateMinute = function (n) { return Date._validate(n, 0, 59, "minutes"); }; Date.validateHour = function (n) { return Date._validate(n, 0, 23, "hours"); }; Date.validateDay = function (n, year, month) { return Date._validate(n, 1, Date.getDaysInMonth(year, month), "days"); }; Date.validateMonth = function (n) { return Date._validate(n, 0, 11, "months"); }; Date.validateYear = function (n) { return Date._validate(n, 1, 9999, "seconds"); }; Date.prototype.set = function (config) {
    var x = config; if (!x.millisecond && x.millisecond !== 0) { x.millisecond = -1; }
    if (!x.second && x.second !== 0) { x.second = -1; }
    if (!x.minute && x.minute !== 0) { x.minute = -1; }
    if (!x.hour && x.hour !== 0) { x.hour = -1; }
    if (!x.day && x.day !== 0) { x.day = -1; }
    if (!x.month && x.month !== 0) { x.month = -1; }
    if (!x.year && x.year !== 0) { x.year = -1; }
    if (x.millisecond != -1 && Date.validateMillisecond(x.millisecond)) { this.addMilliseconds(x.millisecond - this.getMilliseconds()); }
    if (x.second != -1 && Date.validateSecond(x.second)) { this.addSeconds(x.second - this.getSeconds()); }
    if (x.minute != -1 && Date.validateMinute(x.minute)) { this.addMinutes(x.minute - this.getMinutes()); }
    if (x.hour != -1 && Date.validateHour(x.hour)) { this.addHours(x.hour - this.getHours()); }
    if (x.month !== -1 && Date.validateMonth(x.month)) { this.addMonths(x.month - this.getMonth()); }
    if (x.year != -1 && Date.validateYear(x.year)) { this.addYears(x.year - this.getFullYear()); }
    if (x.day != -1 && Date.validateDay(x.day, this.getFullYear(), this.getMonth())) { this.addDays(x.day - this.getDate()); }
    if (x.timezone) { this.setTimezone(x.timezone); }
    if (x.timezoneOffset) { this.setTimezoneOffset(x.timezoneOffset); }
    return this;
}; Date.prototype.clearTime = function () { this.setHours(0); this.setMinutes(0); this.setSeconds(0); this.setMilliseconds(0); return this; }; Date.prototype.isLeapYear = function () { var y = this.getFullYear(); return (((y % 4 === 0) && (y % 100 !== 0)) || (y % 400 === 0)); }; Date.prototype.isWeekday = function () { return !(this.is().sat() || this.is().sun()); }; Date.prototype.getDaysInMonth = function () { return Date.getDaysInMonth(this.getFullYear(), this.getMonth()); }; Date.prototype.moveToFirstDayOfMonth = function () { return this.set({ day: 1 }); }; Date.prototype.moveToLastDayOfMonth = function () { return this.set({ day: this.getDaysInMonth() }); }; Date.prototype.moveToDayOfWeek = function (day, orient) { var diff = (day - this.getDay() + 7 * (orient || +1)) % 7; return this.addDays((diff === 0) ? diff += 7 * (orient || +1) : diff); }; Date.prototype.moveToMonth = function (month, orient) { var diff = (month - this.getMonth() + 12 * (orient || +1)) % 12; return this.addMonths((diff === 0) ? diff += 12 * (orient || +1) : diff); }; Date.prototype.getDayOfYear = function () { return Math.floor((this - new Date(this.getFullYear(), 0, 1)) / 86400000); }; Date.prototype.getWeekOfYear = function (firstDayOfWeek) {
    var y = this.getFullYear(), m = this.getMonth(), d = this.getDate(); var dow = firstDayOfWeek || Date.CultureInfo.firstDayOfWeek; var offset = 7 + 1 - new Date(y, 0, 1).getDay(); if (offset == 8) { offset = 1; }
    var daynum = ((Date.UTC(y, m, d, 0, 0, 0) - Date.UTC(y, 0, 1, 0, 0, 0)) / 86400000) + 1; var w = Math.floor((daynum - offset + 7) / 7); if (w === dow) { y--; var prevOffset = 7 + 1 - new Date(y, 0, 1).getDay(); if (prevOffset == 2 || prevOffset == 8) { w = 53; } else { w = 52; } }
    return w;
}; Date.prototype.isDST = function () { console.log('isDST'); return this.toString().match(/(E|C|M|P)(S|D)T/)[2] == "D"; }; Date.prototype.getTimezone = function () { return Date.getTimezoneAbbreviation(this.getUTCOffset, this.isDST()); }; Date.prototype.setTimezoneOffset = function (s) { var here = this.getTimezoneOffset(), there = Number(s) * -6 / 10; this.addMinutes(there - here); return this; }; Date.prototype.setTimezone = function (s) { return this.setTimezoneOffset(Date.getTimezoneOffset(s)); }; Date.prototype.getUTCOffset = function () { var n = this.getTimezoneOffset() * -10 / 6, r; if (n < 0) { r = (n - 10000).toString(); return r[0] + r.substr(2); } else { r = (n + 10000).toString(); return "+" + r.substr(1); } }; Date.prototype.getDayName = function (abbrev) { return abbrev ? Date.CultureInfo.abbreviatedDayNames[this.getDay()] : Date.CultureInfo.dayNames[this.getDay()]; }; Date.prototype.getMonthName = function (abbrev) { return abbrev ? Date.CultureInfo.abbreviatedMonthNames[this.getMonth()] : Date.CultureInfo.monthNames[this.getMonth()]; }; Date.prototype._toString = Date.prototype.toString; Date.prototype.toString = function (format) { var self = this; var p = function p(s) { return (s.toString().length == 1) ? "0" + s : s; }; return format ? format.replace(/dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?/g, function (format) { switch (format) { case "hh": return p(self.getHours() < 13 ? self.getHours() : (self.getHours() - 12)); case "h": return self.getHours() < 13 ? self.getHours() : (self.getHours() - 12); case "HH": return p(self.getHours()); case "H": return self.getHours(); case "mm": return p(self.getMinutes()); case "m": return self.getMinutes(); case "ss": return p(self.getSeconds()); case "s": return self.getSeconds(); case "yyyy": return self.getFullYear(); case "yy": return self.getFullYear().toString().substring(2, 4); case "dddd": return self.getDayName(); case "ddd": return self.getDayName(true); case "dd": return p(self.getDate()); case "d": return self.getDate().toString(); case "MMMM": return self.getMonthName(); case "MMM": return self.getMonthName(true); case "MM": return p((self.getMonth() + 1)); case "M": return self.getMonth() + 1; case "t": return self.getHours() < 12 ? Date.CultureInfo.amDesignator.substring(0, 1) : Date.CultureInfo.pmDesignator.substring(0, 1); case "tt": return self.getHours() < 12 ? Date.CultureInfo.amDesignator : Date.CultureInfo.pmDesignator; case "zzz": case "zz": case "z": return ""; } }) : this._toString(); };
Date.now = function () { return new Date(); }; Date.today = function () { return Date.now().clearTime(); }; Date.prototype._orient = +1; Date.prototype.next = function () { this._orient = +1; return this; }; Date.prototype.last = Date.prototype.prev = Date.prototype.previous = function () { this._orient = -1; return this; }; Date.prototype._is = false; Date.prototype.is = function () { this._is = true; return this; }; Number.prototype._dateElement = "day"; Number.prototype.fromNow = function () { var c = {}; c[this._dateElement] = this; return Date.now().add(c); }; Number.prototype.ago = function () { var c = {}; c[this._dateElement] = this * -1; return Date.now().add(c); }; (function () {
    var $D = Date.prototype, $N = Number.prototype; var dx = ("sunday monday tuesday wednesday thursday friday saturday").split(/\s/), mx = ("january february march april may june july august september october november december").split(/\s/), px = ("Millisecond Second Minute Hour Day Week Month Year").split(/\s/), de; var df = function (n) {
        return function () {
            if (this._is) { this._is = false; return this.getDay() == n; }
            return this.moveToDayOfWeek(n, this._orient);
        };
    }; for (var i = 0; i < dx.length; i++) { $D[dx[i]] = $D[dx[i].substring(0, 3)] = df(i); }
    var mf = function (n) {
        return function () {
            if (this._is) { this._is = false; return this.getMonth() === n; }
            return this.moveToMonth(n, this._orient);
        };
    }; for (var j = 0; j < mx.length; j++) { $D[mx[j]] = $D[mx[j].substring(0, 3)] = mf(j); }
    var ef = function (j) {
        return function () {
            if (j.substring(j.length - 1) != "s") { j += "s"; }
            return this["add" + j](this._orient);
        };
    }; var nf = function (n) { return function () { this._dateElement = n; return this; }; }; for (var k = 0; k < px.length; k++) { de = px[k].toLowerCase(); $D[de] = $D[de + "s"] = ef(px[k]); $N[de] = $N[de + "s"] = nf(de); }
}()); Date.prototype.toJSONString = function () { return this.toString("yyyy-MM-ddThh:mm:ssZ"); }; Date.prototype.toShortDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortDatePattern); }; Date.prototype.toLongDateString = function () { return this.toString(Date.CultureInfo.formatPatterns.longDatePattern); }; Date.prototype.toShortTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.shortTimePattern); }; Date.prototype.toLongTimeString = function () { return this.toString(Date.CultureInfo.formatPatterns.longTimePattern); }; Date.prototype.getOrdinal = function () { switch (this.getDate()) { case 1: case 21: case 31: return "st"; case 2: case 22: return "nd"; case 3: case 23: return "rd"; default: return "th"; } };
(function () {
    Date.Parsing = { Exception: function (s) { this.message = "Parse error at '" + s.substring(0, 10) + " ...'"; } }; var $P = Date.Parsing; var _ = $P.Operators = {
        rtoken: function (r) { return function (s) { var mx = s.match(r); if (mx) { return ([mx[0], s.substring(mx[0].length)]); } else { throw new $P.Exception(s); } }; }, token: function (s) { return function (s) { return _.rtoken(new RegExp("^\s*" + s + "\s*"))(s); }; }, stoken: function (s) { return _.rtoken(new RegExp("^" + s)); }, until: function (p) {
            return function (s) {
                var qx = [], rx = null; while (s.length) {
                    try { rx = p.call(this, s); } catch (e) { qx.push(rx[0]); s = rx[1]; continue; }
                    break;
                }
                return [qx, s];
            };
        }, many: function (p) {
            return function (s) {
                var rx = [], r = null; while (s.length) {
                    try { r = p.call(this, s); } catch (e) { return [rx, s]; }
                    rx.push(r[0]); s = r[1];
                }
                return [rx, s];
            };
        }, optional: function (p) {
            return function (s) {
                var r = null; try { r = p.call(this, s); } catch (e) { return [null, s]; }
                return [r[0], r[1]];
            };
        }, not: function (p) {
            return function (s) {
                try { p.call(this, s); } catch (e) { return [null, s]; }
                throw new $P.Exception(s);
            };
        }, ignore: function (p) { return p ? function (s) { var r = null; r = p.call(this, s); return [null, r[1]]; } : null; }, product: function () {
            var px = arguments[0], qx = Array.prototype.slice.call(arguments, 1), rx = []; for (var i = 0; i < px.length; i++) { rx.push(_.each(px[i], qx)); }
            return rx;
        }, cache: function (rule) {
            var cache = {}, r = null; return function (s) {
                try { r = cache[s] = (cache[s] || rule.call(this, s)); } catch (e) { r = cache[s] = e; }
                if (r instanceof $P.Exception) { throw r; } else { return r; }
            };
        }, any: function () {
            var px = arguments; return function (s) {
                var r = null; for (var i = 0; i < px.length; i++) {
                    if (px[i] == null) { continue; }
                    try { r = (px[i].call(this, s)); } catch (e) { r = null; }
                    if (r) { return r; }
                }
                throw new $P.Exception(s);
            };
        }, each: function () {
            var px = arguments; return function (s) {
                var rx = [], r = null; for (var i = 0; i < px.length; i++) {
                    if (px[i] == null) { continue; }
                    try { r = (px[i].call(this, s)); } catch (e) { throw new $P.Exception(s); }
                    rx.push(r[0]); s = r[1];
                }
                return [rx, s];
            };
        }, all: function () { var px = arguments, _ = _; return _.each(_.optional(px)); }, sequence: function (px, d, c) {
            d = d || _.rtoken(/^\s*/); c = c || null; if (px.length == 1) { return px[0]; }
            return function (s) {
                var r = null, q = null; var rx = []; for (var i = 0; i < px.length; i++) {
                    try { r = px[i].call(this, s); } catch (e) { break; }
                    rx.push(r[0]); try { q = d.call(this, r[1]); } catch (ex) { q = null; break; }
                    s = q[1];
                }
                if (!r) { throw new $P.Exception(s); }
                if (q) { throw new $P.Exception(q[1]); }
                if (c) { try { r = c.call(this, r[1]); } catch (ey) { throw new $P.Exception(r[1]); } }
                return [rx, (r ? r[1] : s)];
            };
        }, between: function (d1, p, d2) { d2 = d2 || d1; var _fn = _.each(_.ignore(d1), p, _.ignore(d2)); return function (s) { var rx = _fn.call(this, s); return [[rx[0][0], r[0][2]], rx[1]]; }; }, list: function (p, d, c) { d = d || _.rtoken(/^\s*/); c = c || null; return (p instanceof Array ? _.each(_.product(p.slice(0, -1), _.ignore(d)), p.slice(-1), _.ignore(c)) : _.each(_.many(_.each(p, _.ignore(d))), px, _.ignore(c))); }, set: function (px, d, c) {
            d = d || _.rtoken(/^\s*/); c = c || null; return function (s) {
                var r = null, p = null, q = null, rx = null, best = [[], s], last = false; for (var i = 0; i < px.length; i++) {
                    q = null; p = null; r = null; last = (px.length == 1); try { r = px[i].call(this, s); } catch (e) { continue; }
                    rx = [[r[0]], r[1]]; if (r[1].length > 0 && !last) { try { q = d.call(this, r[1]); } catch (ex) { last = true; } } else { last = true; }
                    if (!last && q[1].length === 0) { last = true; }
                    if (!last) {
                        var qx = []; for (var j = 0; j < px.length; j++) { if (i != j) { qx.push(px[j]); } }
                        p = _.set(qx, d).call(this, q[1]); if (p[0].length > 0) { rx[0] = rx[0].concat(p[0]); rx[1] = p[1]; }
                    }
                    if (rx[1].length < best[1].length) { best = rx; }
                    if (best[1].length === 0) { break; }
                }
                if (best[0].length === 0) { return best; }
                if (c) {
                    try { q = c.call(this, best[1]); } catch (ey) { throw new $P.Exception(best[1]); }
                    best[1] = q[1];
                }
                return best;
            };
        }, forward: function (gr, fname) { return function (s) { return gr[fname].call(this, s); }; }, replace: function (rule, repl) { return function (s) { var r = rule.call(this, s); return [repl, r[1]]; }; }, process: function (rule, fn) { return function (s) { var r = rule.call(this, s); return [fn.call(this, r[0]), r[1]]; }; }, min: function (min, rule) {
            return function (s) {
                var rx = rule.call(this, s); if (rx[0].length < min) { throw new $P.Exception(s); }
                return rx;
            };
        }
    }; var _generator = function (op) {
        return function () {
            var args = null, rx = []; if (arguments.length > 1) { args = Array.prototype.slice.call(arguments); } else if (arguments[0] instanceof Array) { args = arguments[0]; }
            if (args) { for (var i = 0, px = args.shift() ; i < px.length; i++) { args.unshift(px[i]); rx.push(op.apply(null, args)); args.shift(); return rx; } } else { return op.apply(null, arguments); }
        };
    }; var gx = "optional not ignore cache".split(/\s/); for (var i = 0; i < gx.length; i++) { _[gx[i]] = _generator(_[gx[i]]); }
    var _vector = function (op) { return function () { if (arguments[0] instanceof Array) { return op.apply(null, arguments[0]); } else { return op.apply(null, arguments); } }; }; var vx = "each any all".split(/\s/); for (var j = 0; j < vx.length; j++) { _[vx[j]] = _vector(_[vx[j]]); }
}()); (function () {
    var flattenAndCompact = function (ax) {
        var rx = []; for (var i = 0; i < ax.length; i++) { if (ax[i] instanceof Array) { rx = rx.concat(flattenAndCompact(ax[i])); } else { if (ax[i]) { rx.push(ax[i]); } } }
        return rx;
    }; Date.Grammar = {}; Date.Translator = {
        hour: function (s) { return function () { this.hour = Number(s); }; }, minute: function (s) { return function () { this.minute = Number(s); }; }, second: function (s) { return function () { this.second = Number(s); }; }, meridian: function (s) { return function () { this.meridian = s.slice(0, 1).toLowerCase(); }; }, timezone: function (s) { return function () { var n = s.replace(/[^\d\+\-]/g, ""); if (n.length) { this.timezoneOffset = Number(n); } else { this.timezone = s.toLowerCase(); } }; }, day: function (x) { var s = x[0]; return function () { this.day = Number(s.match(/\d+/)[0]); }; }, month: function (s) { return function () { this.month = ((s.length == 3) ? Date.getMonthNumberFromName(s) : (Number(s) - 1)); }; }, year: function (s) { return function () { var n = Number(s); this.year = ((s.length > 2) ? n : (n + (((n + 2000) < Date.CultureInfo.twoDigitYearMax) ? 2000 : 1900))); }; }, rday: function (s) { return function () { switch (s) { case "yesterday": this.days = -1; break; case "tomorrow": this.days = 1; break; case "today": this.days = 0; break; case "now": this.days = 0; this.now = true; break; } }; }, finishExact: function (x) {
            x = (x instanceof Array) ? x : [x]; var now = new Date(); this.year = now.getFullYear(); this.month = now.getMonth(); this.day = 1; this.hour = 0; this.minute = 0; this.second = 0; for (var i = 0; i < x.length; i++) { if (x[i]) { x[i].call(this); } }
            this.hour = (this.meridian == "p" && this.hour < 13) ? this.hour + 12 : this.hour; if (this.day > Date.getDaysInMonth(this.year, this.month)) { throw new RangeError(this.day + " is not a valid value for days."); }
            var r = new Date(this.year, this.month, this.day, this.hour, this.minute, this.second); if (this.timezone) { r.set({ timezone: this.timezone }); } else if (this.timezoneOffset) { r.set({ timezoneOffset: this.timezoneOffset }); }
            return r;
        }, finish: function (x) {
            x = (x instanceof Array) ? flattenAndCompact(x) : [x]; if (x.length === 0) { return null; }
            for (var i = 0; i < x.length; i++) { if (typeof x[i] == "function") { x[i].call(this); } }
            if (this.now) { return new Date(); }
            var today = Date.today(); var method = null; var expression = !!(this.days != null || this.orient || this.operator); if (expression) {
                var gap, mod, orient; orient = ((this.orient == "past" || this.operator == "subtract") ? -1 : 1); if (this.weekday) { this.unit = "day"; gap = (Date.getDayNumberFromName(this.weekday) - today.getDay()); mod = 7; this.days = gap ? ((gap + (orient * mod)) % mod) : (orient * mod); }
                if (this.month) { this.unit = "month"; gap = (this.month - today.getMonth()); mod = 12; this.months = gap ? ((gap + (orient * mod)) % mod) : (orient * mod); this.month = null; }
                if (!this.unit) { this.unit = "day"; }
                if (this[this.unit + "s"] == null || this.operator != null) {
                    if (!this.value) { this.value = 1; }
                    if (this.unit == "week") { this.unit = "day"; this.value = this.value * 7; }
                    this[this.unit + "s"] = this.value * orient;
                }
                return today.add(this);
            } else {
                if (this.meridian && this.hour) { this.hour = (this.hour < 13 && this.meridian == "p") ? this.hour + 12 : this.hour; }
                if (this.weekday && !this.day) { this.day = (today.addDays((Date.getDayNumberFromName(this.weekday) - today.getDay()))).getDate(); }
                if (this.month && !this.day) { this.day = 1; }
                return today.set(this);
            }
        }
    }; var _ = Date.Parsing.Operators, g = Date.Grammar, t = Date.Translator, _fn; g.datePartDelimiter = _.rtoken(/^([\s\-\.\,\/\x27]+)/); g.timePartDelimiter = _.stoken(":"); g.whiteSpace = _.rtoken(/^\s*/); g.generalDelimiter = _.rtoken(/^(([\s\,]|at|on)+)/); var _C = {}; g.ctoken = function (keys) {
        var fn = _C[keys]; if (!fn) {
            var c = Date.CultureInfo.regexPatterns; var kx = keys.split(/\s+/), px = []; for (var i = 0; i < kx.length; i++) { px.push(_.replace(_.rtoken(c[kx[i]]), kx[i])); }
            fn = _C[keys] = _.any.apply(null, px);
        }
        return fn;
    }; g.ctoken2 = function (key) { return _.rtoken(Date.CultureInfo.regexPatterns[key]); }; g.h = _.cache(_.process(_.rtoken(/^(0[0-9]|1[0-2]|[1-9])/), t.hour)); g.hh = _.cache(_.process(_.rtoken(/^(0[0-9]|1[0-2])/), t.hour)); g.H = _.cache(_.process(_.rtoken(/^([0-1][0-9]|2[0-3]|[0-9])/), t.hour)); g.HH = _.cache(_.process(_.rtoken(/^([0-1][0-9]|2[0-3])/), t.hour)); g.m = _.cache(_.process(_.rtoken(/^([0-5][0-9]|[0-9])/), t.minute)); g.mm = _.cache(_.process(_.rtoken(/^[0-5][0-9]/), t.minute)); g.s = _.cache(_.process(_.rtoken(/^([0-5][0-9]|[0-9])/), t.second)); g.ss = _.cache(_.process(_.rtoken(/^[0-5][0-9]/), t.second)); g.hms = _.cache(_.sequence([g.H, g.mm, g.ss], g.timePartDelimiter)); g.t = _.cache(_.process(g.ctoken2("shortMeridian"), t.meridian)); g.tt = _.cache(_.process(g.ctoken2("longMeridian"), t.meridian)); g.z = _.cache(_.process(_.rtoken(/^(\+|\-)?\s*\d\d\d\d?/), t.timezone)); g.zz = _.cache(_.process(_.rtoken(/^(\+|\-)\s*\d\d\d\d/), t.timezone)); g.zzz = _.cache(_.process(g.ctoken2("timezone"), t.timezone)); g.timeSuffix = _.each(_.ignore(g.whiteSpace), _.set([g.tt, g.zzz])); g.time = _.each(_.optional(_.ignore(_.stoken("T"))), g.hms, g.timeSuffix); g.d = _.cache(_.process(_.each(_.rtoken(/^([0-2]\d|3[0-1]|\d)/), _.optional(g.ctoken2("ordinalSuffix"))), t.day)); g.dd = _.cache(_.process(_.each(_.rtoken(/^([0-2]\d|3[0-1])/), _.optional(g.ctoken2("ordinalSuffix"))), t.day)); g.ddd = g.dddd = _.cache(_.process(g.ctoken("sun mon tue wed thu fri sat"), function (s) { return function () { this.weekday = s; }; })); g.M = _.cache(_.process(_.rtoken(/^(1[0-2]|0\d|\d)/), t.month)); g.MM = _.cache(_.process(_.rtoken(/^(1[0-2]|0\d)/), t.month)); g.MMM = g.MMMM = _.cache(_.process(g.ctoken("jan feb mar apr may jun jul aug sep oct nov dec"), t.month)); g.y = _.cache(_.process(_.rtoken(/^(\d\d?)/), t.year)); g.yy = _.cache(_.process(_.rtoken(/^(\d\d)/), t.year)); g.yyy = _.cache(_.process(_.rtoken(/^(\d\d?\d?\d?)/), t.year)); g.yyyy = _.cache(_.process(_.rtoken(/^(\d\d\d\d)/), t.year)); _fn = function () { return _.each(_.any.apply(null, arguments), _.not(g.ctoken2("timeContext"))); }; g.day = _fn(g.d, g.dd); g.month = _fn(g.M, g.MMM); g.year = _fn(g.yyyy, g.yy); g.orientation = _.process(g.ctoken("past future"), function (s) { return function () { this.orient = s; }; }); g.operator = _.process(g.ctoken("add subtract"), function (s) { return function () { this.operator = s; }; }); g.rday = _.process(g.ctoken("yesterday tomorrow today now"), t.rday); g.unit = _.process(g.ctoken("minute hour day week month year"), function (s) { return function () { this.unit = s; }; }); g.value = _.process(_.rtoken(/^\d\d?(st|nd|rd|th)?/), function (s) { return function () { this.value = s.replace(/\D/g, ""); }; }); g.expression = _.set([g.rday, g.operator, g.value, g.unit, g.orientation, g.ddd, g.MMM]); _fn = function () { return _.set(arguments, g.datePartDelimiter); }; g.mdy = _fn(g.ddd, g.month, g.day, g.year); g.ymd = _fn(g.ddd, g.year, g.month, g.day); g.dmy = _fn(g.ddd, g.day, g.month, g.year); g.date = function (s) { return ((g[Date.CultureInfo.dateElementOrder] || g.mdy).call(this, s)); }; g.format = _.process(_.many(_.any(_.process(_.rtoken(/^(dd?d?d?|MM?M?M?|yy?y?y?|hh?|HH?|mm?|ss?|tt?|zz?z?)/), function (fmt) { if (g[fmt]) { return g[fmt]; } else { throw Date.Parsing.Exception(fmt); } }), _.process(_.rtoken(/^[^dMyhHmstz]+/), function (s) { return _.ignore(_.stoken(s)); }))), function (rules) { return _.process(_.each.apply(null, rules), t.finishExact); }); var _F = {}; var _get = function (f) { return _F[f] = (_F[f] || g.format(f)[0]); }; g.formats = function (fx) {
        if (fx instanceof Array) {
            var rx = []; for (var i = 0; i < fx.length; i++) { rx.push(_get(fx[i])); }
            return _.any.apply(null, rx);
        } else { return _get(fx); }
    }; g._formats = g.formats(["yyyy-MM-ddTHH:mm:ss", "ddd, MMM dd, yyyy H:mm:ss tt", "ddd MMM d yyyy HH:mm:ss zzz", "d"]); g._start = _.process(_.set([g.date, g.time, g.expression], g.generalDelimiter, g.whiteSpace), t.finish); g.start = function (s) {
        try { var r = g._formats.call({}, s); if (r[1].length === 0) { return r; } } catch (e) { }
        return g._start.call({}, s);
    };
}()); Date._parse = Date.parse; Date.parse = function (s) {
    var r = null; if (!s) { return null; }
    try { r = Date.Grammar.start.call({}, s); } catch (e) { return null; }
    return ((r[1].length === 0) ? r[0] : null);
}; Date.getParseFunction = function (fx) {
    var fn = Date.Grammar.formats(fx); return function (s) {
        var r = null; try { r = fn.call({}, s); } catch (e) { return null; }
        return ((r[1].length === 0) ? r[0] : null);
    };
}; Date.parseExact = function (s, fx) { return Date.getParseFunction(fx)(s); };



//! moment.js
//! version : 2.6.0
//! authors : Tim Wood, Iskren Chernev, Moment.js contributors
//! license : MIT
//! momentjs.com
(function (a) { function b() { return { empty: !1, unusedTokens: [], unusedInput: [], overflow: -2, charsLeftOver: 0, nullInput: !1, invalidMonth: null, invalidFormat: !1, userInvalidated: !1, iso: !1 } } function c(a, b) { function c() { ib.suppressDeprecationWarnings === !1 && "undefined" != typeof console && console.warn && console.warn("Deprecation warning: " + a) } var d = !0; return i(function () { return d && (c(), d = !1), b.apply(this, arguments) }, b) } function d(a, b) { return function (c) { return l(a.call(this, c), b) } } function e(a, b) { return function (c) { return this.lang().ordinal(a.call(this, c), b) } } function f() { } function g(a) { y(a), i(this, a) } function h(a) { var b = r(a), c = b.year || 0, d = b.quarter || 0, e = b.month || 0, f = b.week || 0, g = b.day || 0, h = b.hour || 0, i = b.minute || 0, j = b.second || 0, k = b.millisecond || 0; this._milliseconds = +k + 1e3 * j + 6e4 * i + 36e5 * h, this._days = +g + 7 * f, this._months = +e + 3 * d + 12 * c, this._data = {}, this._bubble() } function i(a, b) { for (var c in b) b.hasOwnProperty(c) && (a[c] = b[c]); return b.hasOwnProperty("toString") && (a.toString = b.toString), b.hasOwnProperty("valueOf") && (a.valueOf = b.valueOf), a } function j(a) { var b, c = {}; for (b in a) a.hasOwnProperty(b) && wb.hasOwnProperty(b) && (c[b] = a[b]); return c } function k(a) { return 0 > a ? Math.ceil(a) : Math.floor(a) } function l(a, b, c) { for (var d = "" + Math.abs(a), e = a >= 0; d.length < b;) d = "0" + d; return (e ? c ? "+" : "" : "-") + d } function m(a, b, c, d) { var e = b._milliseconds, f = b._days, g = b._months; d = null == d ? !0 : d, e && a._d.setTime(+a._d + e * c), f && db(a, "Date", cb(a, "Date") + f * c), g && bb(a, cb(a, "Month") + g * c), d && ib.updateOffset(a, f || g) } function n(a) { return "[object Array]" === Object.prototype.toString.call(a) } function o(a) { return "[object Date]" === Object.prototype.toString.call(a) || a instanceof Date } function p(a, b, c) { var d, e = Math.min(a.length, b.length), f = Math.abs(a.length - b.length), g = 0; for (d = 0; e > d; d++) (c && a[d] !== b[d] || !c && t(a[d]) !== t(b[d])) && g++; return g + f } function q(a) { if (a) { var b = a.toLowerCase().replace(/(.)s$/, "$1"); a = Zb[a] || $b[b] || b } return a } function r(a) { var b, c, d = {}; for (c in a) a.hasOwnProperty(c) && (b = q(c), b && (d[b] = a[c])); return d } function s(b) { var c, d; if (0 === b.indexOf("week")) c = 7, d = "day"; else { if (0 !== b.indexOf("month")) return; c = 12, d = "month" } ib[b] = function (e, f) { var g, h, i = ib.fn._lang[b], j = []; if ("number" == typeof e && (f = e, e = a), h = function (a) { var b = ib().utc().set(d, a); return i.call(ib.fn._lang, b, e || "") }, null != f) return h(f); for (g = 0; c > g; g++) j.push(h(g)); return j } } function t(a) { var b = +a, c = 0; return 0 !== b && isFinite(b) && (c = b >= 0 ? Math.floor(b) : Math.ceil(b)), c } function u(a, b) { return new Date(Date.UTC(a, b + 1, 0)).getUTCDate() } function v(a, b, c) { return $(ib([a, 11, 31 + b - c]), b, c).week } function w(a) { return x(a) ? 366 : 365 } function x(a) { return a % 4 === 0 && a % 100 !== 0 || a % 400 === 0 } function y(a) { var b; a._a && -2 === a._pf.overflow && (b = a._a[pb] < 0 || a._a[pb] > 11 ? pb : a._a[qb] < 1 || a._a[qb] > u(a._a[ob], a._a[pb]) ? qb : a._a[rb] < 0 || a._a[rb] > 23 ? rb : a._a[sb] < 0 || a._a[sb] > 59 ? sb : a._a[tb] < 0 || a._a[tb] > 59 ? tb : a._a[ub] < 0 || a._a[ub] > 999 ? ub : -1, a._pf._overflowDayOfYear && (ob > b || b > qb) && (b = qb), a._pf.overflow = b) } function z(a) { return null == a._isValid && (a._isValid = !isNaN(a._d.getTime()) && a._pf.overflow < 0 && !a._pf.empty && !a._pf.invalidMonth && !a._pf.nullInput && !a._pf.invalidFormat && !a._pf.userInvalidated, a._strict && (a._isValid = a._isValid && 0 === a._pf.charsLeftOver && 0 === a._pf.unusedTokens.length)), a._isValid } function A(a) { return a ? a.toLowerCase().replace("_", "-") : a } function B(a, b) { return b._isUTC ? ib(a).zone(b._offset || 0) : ib(a).local() } function C(a, b) { return b.abbr = a, vb[a] || (vb[a] = new f), vb[a].set(b), vb[a] } function D(a) { delete vb[a] } function E(a) { var b, c, d, e, f = 0, g = function (a) { if (!vb[a] && xb) try { require("./lang/" + a) } catch (b) { } return vb[a] }; if (!a) return ib.fn._lang; if (!n(a)) { if (c = g(a)) return c; a = [a] } for (; f < a.length;) { for (e = A(a[f]).split("-"), b = e.length, d = A(a[f + 1]), d = d ? d.split("-") : null; b > 0;) { if (c = g(e.slice(0, b).join("-"))) return c; if (d && d.length >= b && p(e, d, !0) >= b - 1) break; b-- } f++ } return ib.fn._lang } function F(a) { return a.match(/\[[\s\S]/) ? a.replace(/^\[|\]$/g, "") : a.replace(/\\/g, "") } function G(a) { var b, c, d = a.match(Bb); for (b = 0, c = d.length; c > b; b++) d[b] = cc[d[b]] ? cc[d[b]] : F(d[b]); return function (e) { var f = ""; for (b = 0; c > b; b++) f += d[b] instanceof Function ? d[b].call(e, a) : d[b]; return f } } function H(a, b) { return a.isValid() ? (b = I(b, a.lang()), _b[b] || (_b[b] = G(b)), _b[b](a)) : a.lang().invalidDate() } function I(a, b) { function c(a) { return b.longDateFormat(a) || a } var d = 5; for (Cb.lastIndex = 0; d >= 0 && Cb.test(a) ;) a = a.replace(Cb, c), Cb.lastIndex = 0, d -= 1; return a } function J(a, b) { var c, d = b._strict; switch (a) { case "Q": return Nb; case "DDDD": return Pb; case "YYYY": case "GGGG": case "gggg": return d ? Qb : Fb; case "Y": case "G": case "g": return Sb; case "YYYYYY": case "YYYYY": case "GGGGG": case "ggggg": return d ? Rb : Gb; case "S": if (d) return Nb; case "SS": if (d) return Ob; case "SSS": if (d) return Pb; case "DDD": return Eb; case "MMM": case "MMMM": case "dd": case "ddd": case "dddd": return Ib; case "a": case "A": return E(b._l)._meridiemParse; case "X": return Lb; case "Z": case "ZZ": return Jb; case "T": return Kb; case "SSSS": return Hb; case "MM": case "DD": case "YY": case "GG": case "gg": case "HH": case "hh": case "mm": case "ss": case "ww": case "WW": return d ? Ob : Db; case "M": case "D": case "d": case "H": case "h": case "m": case "s": case "w": case "W": case "e": case "E": return Db; case "Do": return Mb; default: return c = new RegExp(R(Q(a.replace("\\", "")), "i")) } } function K(a) { a = a || ""; var b = a.match(Jb) || [], c = b[b.length - 1] || [], d = (c + "").match(Xb) || ["-", 0, 0], e = +(60 * d[1]) + t(d[2]); return "+" === d[0] ? -e : e } function L(a, b, c) { var d, e = c._a; switch (a) { case "Q": null != b && (e[pb] = 3 * (t(b) - 1)); break; case "M": case "MM": null != b && (e[pb] = t(b) - 1); break; case "MMM": case "MMMM": d = E(c._l).monthsParse(b), null != d ? e[pb] = d : c._pf.invalidMonth = b; break; case "D": case "DD": null != b && (e[qb] = t(b)); break; case "Do": null != b && (e[qb] = t(parseInt(b, 10))); break; case "DDD": case "DDDD": null != b && (c._dayOfYear = t(b)); break; case "YY": e[ob] = ib.parseTwoDigitYear(b); break; case "YYYY": case "YYYYY": case "YYYYYY": e[ob] = t(b); break; case "a": case "A": c._isPm = E(c._l).isPM(b); break; case "H": case "HH": case "h": case "hh": e[rb] = t(b); break; case "m": case "mm": e[sb] = t(b); break; case "s": case "ss": e[tb] = t(b); break; case "S": case "SS": case "SSS": case "SSSS": e[ub] = t(1e3 * ("0." + b)); break; case "X": c._d = new Date(1e3 * parseFloat(b)); break; case "Z": case "ZZ": c._useUTC = !0, c._tzm = K(b); break; case "w": case "ww": case "W": case "WW": case "d": case "dd": case "ddd": case "dddd": case "e": case "E": a = a.substr(0, 1); case "gg": case "gggg": case "GG": case "GGGG": case "GGGGG": a = a.substr(0, 2), b && (c._w = c._w || {}, c._w[a] = b) } } function M(a) { var b, c, d, e, f, g, h, i, j, k, l = []; if (!a._d) { for (d = O(a), a._w && null == a._a[qb] && null == a._a[pb] && (f = function (b) { var c = parseInt(b, 10); return b ? b.length < 3 ? c > 68 ? 1900 + c : 2e3 + c : c : null == a._a[ob] ? ib().weekYear() : a._a[ob] }, g = a._w, null != g.GG || null != g.W || null != g.E ? h = _(f(g.GG), g.W || 1, g.E, 4, 1) : (i = E(a._l), j = null != g.d ? X(g.d, i) : null != g.e ? parseInt(g.e, 10) + i._week.dow : 0, k = parseInt(g.w, 10) || 1, null != g.d && j < i._week.dow && k++, h = _(f(g.gg), k, j, i._week.doy, i._week.dow)), a._a[ob] = h.year, a._dayOfYear = h.dayOfYear), a._dayOfYear && (e = null == a._a[ob] ? d[ob] : a._a[ob], a._dayOfYear > w(e) && (a._pf._overflowDayOfYear = !0), c = W(e, 0, a._dayOfYear), a._a[pb] = c.getUTCMonth(), a._a[qb] = c.getUTCDate()), b = 0; 3 > b && null == a._a[b]; ++b) a._a[b] = l[b] = d[b]; for (; 7 > b; b++) a._a[b] = l[b] = null == a._a[b] ? 2 === b ? 1 : 0 : a._a[b]; l[rb] += t((a._tzm || 0) / 60), l[sb] += t((a._tzm || 0) % 60), a._d = (a._useUTC ? W : V).apply(null, l) } } function N(a) { var b; a._d || (b = r(a._i), a._a = [b.year, b.month, b.day, b.hour, b.minute, b.second, b.millisecond], M(a)) } function O(a) { var b = new Date; return a._useUTC ? [b.getUTCFullYear(), b.getUTCMonth(), b.getUTCDate()] : [b.getFullYear(), b.getMonth(), b.getDate()] } function P(a) { a._a = [], a._pf.empty = !0; var b, c, d, e, f, g = E(a._l), h = "" + a._i, i = h.length, j = 0; for (d = I(a._f, g).match(Bb) || [], b = 0; b < d.length; b++) e = d[b], c = (h.match(J(e, a)) || [])[0], c && (f = h.substr(0, h.indexOf(c)), f.length > 0 && a._pf.unusedInput.push(f), h = h.slice(h.indexOf(c) + c.length), j += c.length), cc[e] ? (c ? a._pf.empty = !1 : a._pf.unusedTokens.push(e), L(e, c, a)) : a._strict && !c && a._pf.unusedTokens.push(e); a._pf.charsLeftOver = i - j, h.length > 0 && a._pf.unusedInput.push(h), a._isPm && a._a[rb] < 12 && (a._a[rb] += 12), a._isPm === !1 && 12 === a._a[rb] && (a._a[rb] = 0), M(a), y(a) } function Q(a) { return a.replace(/\\(\[)|\\(\])|\[([^\]\[]*)\]|\\(.)/g, function (a, b, c, d, e) { return b || c || d || e }) } function R(a) { return a.replace(/[-\/\\^$*+?.()|[\]{}]/g, "\\$&") } function S(a) { var c, d, e, f, g; if (0 === a._f.length) return a._pf.invalidFormat = !0, void (a._d = new Date(0 / 0)); for (f = 0; f < a._f.length; f++) g = 0, c = i({}, a), c._pf = b(), c._f = a._f[f], P(c), z(c) && (g += c._pf.charsLeftOver, g += 10 * c._pf.unusedTokens.length, c._pf.score = g, (null == e || e > g) && (e = g, d = c)); i(a, d || c) } function T(a) { var b, c, d = a._i, e = Tb.exec(d); if (e) { for (a._pf.iso = !0, b = 0, c = Vb.length; c > b; b++) if (Vb[b][1].exec(d)) { a._f = Vb[b][0] + (e[6] || " "); break } for (b = 0, c = Wb.length; c > b; b++) if (Wb[b][1].exec(d)) { a._f += Wb[b][0]; break } d.match(Jb) && (a._f += "Z"), P(a) } else ib.createFromInputFallback(a) } function U(b) { var c = b._i, d = yb.exec(c); c === a ? b._d = new Date : d ? b._d = new Date(+d[1]) : "string" == typeof c ? T(b) : n(c) ? (b._a = c.slice(0), M(b)) : o(c) ? b._d = new Date(+c) : "object" == typeof c ? N(b) : "number" == typeof c ? b._d = new Date(c) : ib.createFromInputFallback(b) } function V(a, b, c, d, e, f, g) { var h = new Date(a, b, c, d, e, f, g); return 1970 > a && h.setFullYear(a), h } function W(a) { var b = new Date(Date.UTC.apply(null, arguments)); return 1970 > a && b.setUTCFullYear(a), b } function X(a, b) { if ("string" == typeof a) if (isNaN(a)) { if (a = b.weekdaysParse(a), "number" != typeof a) return null } else a = parseInt(a, 10); return a } function Y(a, b, c, d, e) { return e.relativeTime(b || 1, !!c, a, d) } function Z(a, b, c) { var d = nb(Math.abs(a) / 1e3), e = nb(d / 60), f = nb(e / 60), g = nb(f / 24), h = nb(g / 365), i = 45 > d && ["s", d] || 1 === e && ["m"] || 45 > e && ["mm", e] || 1 === f && ["h"] || 22 > f && ["hh", f] || 1 === g && ["d"] || 25 >= g && ["dd", g] || 45 >= g && ["M"] || 345 > g && ["MM", nb(g / 30)] || 1 === h && ["y"] || ["yy", h]; return i[2] = b, i[3] = a > 0, i[4] = c, Y.apply({}, i) } function $(a, b, c) { var d, e = c - b, f = c - a.day(); return f > e && (f -= 7), e - 7 > f && (f += 7), d = ib(a).add("d", f), { week: Math.ceil(d.dayOfYear() / 7), year: d.year() } } function _(a, b, c, d, e) { var f, g, h = W(a, 0, 1).getUTCDay(); return c = null != c ? c : e, f = e - h + (h > d ? 7 : 0) - (e > h ? 7 : 0), g = 7 * (b - 1) + (c - e) + f + 1, { year: g > 0 ? a : a - 1, dayOfYear: g > 0 ? g : w(a - 1) + g } } function ab(b) { var c = b._i, d = b._f; return null === c || d === a && "" === c ? ib.invalid({ nullInput: !0 }) : ("string" == typeof c && (b._i = c = E().preparse(c)), ib.isMoment(c) ? (b = j(c), b._d = new Date(+c._d)) : d ? n(d) ? S(b) : P(b) : U(b), new g(b)) } function bb(a, b) { var c; return "string" == typeof b && (b = a.lang().monthsParse(b), "number" != typeof b) ? a : (c = Math.min(a.date(), u(a.year(), b)), a._d["set" + (a._isUTC ? "UTC" : "") + "Month"](b, c), a) } function cb(a, b) { return a._d["get" + (a._isUTC ? "UTC" : "") + b]() } function db(a, b, c) { return "Month" === b ? bb(a, c) : a._d["set" + (a._isUTC ? "UTC" : "") + b](c) } function eb(a, b) { return function (c) { return null != c ? (db(this, a, c), ib.updateOffset(this, b), this) : cb(this, a) } } function fb(a) { ib.duration.fn[a] = function () { return this._data[a] } } function gb(a, b) { ib.duration.fn["as" + a] = function () { return +this / b } } function hb(a) { "undefined" == typeof ender && (jb = mb.moment, mb.moment = a ? c("Accessing Moment through the global scope is deprecated, and will be removed in an upcoming release.", ib) : ib) } for (var ib, jb, kb, lb = "2.6.0", mb = "undefined" != typeof global ? global : this, nb = Math.round, ob = 0, pb = 1, qb = 2, rb = 3, sb = 4, tb = 5, ub = 6, vb = {}, wb = { _isAMomentObject: null, _i: null, _f: null, _l: null, _strict: null, _isUTC: null, _offset: null, _pf: null, _lang: null }, xb = "undefined" != typeof module && module.exports, yb = /^\/?Date\((\-?\d+)/i, zb = /(\-)?(?:(\d*)\.)?(\d+)\:(\d+)(?:\:(\d+)\.?(\d{3})?)?/, Ab = /^(-)?P(?:(?:([0-9,.]*)Y)?(?:([0-9,.]*)M)?(?:([0-9,.]*)D)?(?:T(?:([0-9,.]*)H)?(?:([0-9,.]*)M)?(?:([0-9,.]*)S)?)?|([0-9,.]*)W)$/, Bb = /(\[[^\[]*\])|(\\)?(Mo|MM?M?M?|Do|DDDo|DD?D?D?|ddd?d?|do?|w[o|w]?|W[o|W]?|Q|YYYYYY|YYYYY|YYYY|YY|gg(ggg?)?|GG(GGG?)?|e|E|a|A|hh?|HH?|mm?|ss?|S{1,4}|X|zz?|ZZ?|.)/g, Cb = /(\[[^\[]*\])|(\\)?(LT|LL?L?L?|l{1,4})/g, Db = /\d\d?/, Eb = /\d{1,3}/, Fb = /\d{1,4}/, Gb = /[+\-]?\d{1,6}/, Hb = /\d+/, Ib = /[0-9]*['a-z\u00A0-\u05FF\u0700-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]+|[\u0600-\u06FF\/]+(\s*?[\u0600-\u06FF]+){1,2}/i, Jb = /Z|[\+\-]\d\d:?\d\d/gi, Kb = /T/i, Lb = /[\+\-]?\d+(\.\d{1,3})?/, Mb = /\d{1,2}/, Nb = /\d/, Ob = /\d\d/, Pb = /\d{3}/, Qb = /\d{4}/, Rb = /[+-]?\d{6}/, Sb = /[+-]?\d+/, Tb = /^\s*(?:[+-]\d{6}|\d{4})-(?:(\d\d-\d\d)|(W\d\d$)|(W\d\d-\d)|(\d\d\d))((T| )(\d\d(:\d\d(:\d\d(\.\d+)?)?)?)?([\+\-]\d\d(?::?\d\d)?|\s*Z)?)?$/, Ub = "YYYY-MM-DDTHH:mm:ssZ", Vb = [["YYYYYY-MM-DD", /[+-]\d{6}-\d{2}-\d{2}/], ["YYYY-MM-DD", /\d{4}-\d{2}-\d{2}/], ["GGGG-[W]WW-E", /\d{4}-W\d{2}-\d/], ["GGGG-[W]WW", /\d{4}-W\d{2}/], ["YYYY-DDD", /\d{4}-\d{3}/]], Wb = [["HH:mm:ss.SSSS", /(T| )\d\d:\d\d:\d\d\.\d+/], ["HH:mm:ss", /(T| )\d\d:\d\d:\d\d/], ["HH:mm", /(T| )\d\d:\d\d/], ["HH", /(T| )\d\d/]], Xb = /([\+\-]|\d\d)/gi, Yb = ("Date|Hours|Minutes|Seconds|Milliseconds".split("|"), { Milliseconds: 1, Seconds: 1e3, Minutes: 6e4, Hours: 36e5, Days: 864e5, Months: 2592e6, Years: 31536e6 }), Zb = { ms: "millisecond", s: "second", m: "minute", h: "hour", d: "day", D: "date", w: "week", W: "isoWeek", M: "month", Q: "quarter", y: "year", DDD: "dayOfYear", e: "weekday", E: "isoWeekday", gg: "weekYear", GG: "isoWeekYear" }, $b = { dayofyear: "dayOfYear", isoweekday: "isoWeekday", isoweek: "isoWeek", weekyear: "weekYear", isoweekyear: "isoWeekYear" }, _b = {}, ac = "DDD w W M D d".split(" "), bc = "M D H h m s w W".split(" "), cc = { M: function () { return this.month() + 1 }, MMM: function (a) { return this.lang().monthsShort(this, a) }, MMMM: function (a) { return this.lang().months(this, a) }, D: function () { return this.date() }, DDD: function () { return this.dayOfYear() }, d: function () { return this.day() }, dd: function (a) { return this.lang().weekdaysMin(this, a) }, ddd: function (a) { return this.lang().weekdaysShort(this, a) }, dddd: function (a) { return this.lang().weekdays(this, a) }, w: function () { return this.week() }, W: function () { return this.isoWeek() }, YY: function () { return l(this.year() % 100, 2) }, YYYY: function () { return l(this.year(), 4) }, YYYYY: function () { return l(this.year(), 5) }, YYYYYY: function () { var a = this.year(), b = a >= 0 ? "+" : "-"; return b + l(Math.abs(a), 6) }, gg: function () { return l(this.weekYear() % 100, 2) }, gggg: function () { return l(this.weekYear(), 4) }, ggggg: function () { return l(this.weekYear(), 5) }, GG: function () { return l(this.isoWeekYear() % 100, 2) }, GGGG: function () { return l(this.isoWeekYear(), 4) }, GGGGG: function () { return l(this.isoWeekYear(), 5) }, e: function () { return this.weekday() }, E: function () { return this.isoWeekday() }, a: function () { return this.lang().meridiem(this.hours(), this.minutes(), !0) }, A: function () { return this.lang().meridiem(this.hours(), this.minutes(), !1) }, H: function () { return this.hours() }, h: function () { return this.hours() % 12 || 12 }, m: function () { return this.minutes() }, s: function () { return this.seconds() }, S: function () { return t(this.milliseconds() / 100) }, SS: function () { return l(t(this.milliseconds() / 10), 2) }, SSS: function () { return l(this.milliseconds(), 3) }, SSSS: function () { return l(this.milliseconds(), 3) }, Z: function () { var a = -this.zone(), b = "+"; return 0 > a && (a = -a, b = "-"), b + l(t(a / 60), 2) + ":" + l(t(a) % 60, 2) }, ZZ: function () { var a = -this.zone(), b = "+"; return 0 > a && (a = -a, b = "-"), b + l(t(a / 60), 2) + l(t(a) % 60, 2) }, z: function () { return this.zoneAbbr() }, zz: function () { return this.zoneName() }, X: function () { return this.unix() }, Q: function () { return this.quarter() } }, dc = ["months", "monthsShort", "weekdays", "weekdaysShort", "weekdaysMin"]; ac.length;) kb = ac.pop(), cc[kb + "o"] = e(cc[kb], kb); for (; bc.length;) kb = bc.pop(), cc[kb + kb] = d(cc[kb], 2); for (cc.DDDD = d(cc.DDD, 3), i(f.prototype, { set: function (a) { var b, c; for (c in a) b = a[c], "function" == typeof b ? this[c] = b : this["_" + c] = b }, _months: "January_February_March_April_May_June_July_August_September_October_November_December".split("_"), months: function (a) { return this._months[a.month()] }, _monthsShort: "Jan_Feb_Mar_Apr_May_Jun_Jul_Aug_Sep_Oct_Nov_Dec".split("_"), monthsShort: function (a) { return this._monthsShort[a.month()] }, monthsParse: function (a) { var b, c, d; for (this._monthsParse || (this._monthsParse = []), b = 0; 12 > b; b++) if (this._monthsParse[b] || (c = ib.utc([2e3, b]), d = "^" + this.months(c, "") + "|^" + this.monthsShort(c, ""), this._monthsParse[b] = new RegExp(d.replace(".", ""), "i")), this._monthsParse[b].test(a)) return b }, _weekdays: "Sunday_Monday_Tuesday_Wednesday_Thursday_Friday_Saturday".split("_"), weekdays: function (a) { return this._weekdays[a.day()] }, _weekdaysShort: "Sun_Mon_Tue_Wed_Thu_Fri_Sat".split("_"), weekdaysShort: function (a) { return this._weekdaysShort[a.day()] }, _weekdaysMin: "Su_Mo_Tu_We_Th_Fr_Sa".split("_"), weekdaysMin: function (a) { return this._weekdaysMin[a.day()] }, weekdaysParse: function (a) { var b, c, d; for (this._weekdaysParse || (this._weekdaysParse = []), b = 0; 7 > b; b++) if (this._weekdaysParse[b] || (c = ib([2e3, 1]).day(b), d = "^" + this.weekdays(c, "") + "|^" + this.weekdaysShort(c, "") + "|^" + this.weekdaysMin(c, ""), this._weekdaysParse[b] = new RegExp(d.replace(".", ""), "i")), this._weekdaysParse[b].test(a)) return b }, _longDateFormat: { LT: "h:mm A", L: "MM/DD/YYYY", LL: "MMMM D YYYY", LLL: "MMMM D YYYY LT", LLLL: "dddd, MMMM D YYYY LT" }, longDateFormat: function (a) { var b = this._longDateFormat[a]; return !b && this._longDateFormat[a.toUpperCase()] && (b = this._longDateFormat[a.toUpperCase()].replace(/MMMM|MM|DD|dddd/g, function (a) { return a.slice(1) }), this._longDateFormat[a] = b), b }, isPM: function (a) { return "p" === (a + "").toLowerCase().charAt(0) }, _meridiemParse: /[ap]\.?m?\.?/i, meridiem: function (a, b, c) { return a > 11 ? c ? "pm" : "PM" : c ? "am" : "AM" }, _calendar: { sameDay: "[Today at] LT", nextDay: "[Tomorrow at] LT", nextWeek: "dddd [at] LT", lastDay: "[Yesterday at] LT", lastWeek: "[Last] dddd [at] LT", sameElse: "L" }, calendar: function (a, b) { var c = this._calendar[a]; return "function" == typeof c ? c.apply(b) : c }, _relativeTime: { future: "in %s", past: "%s ago", s: "a few seconds", m: "a minute", mm: "%d minutes", h: "an hour", hh: "%d hours", d: "a day", dd: "%d days", M: "a month", MM: "%d months", y: "a year", yy: "%d years" }, relativeTime: function (a, b, c, d) { var e = this._relativeTime[c]; return "function" == typeof e ? e(a, b, c, d) : e.replace(/%d/i, a) }, pastFuture: function (a, b) { var c = this._relativeTime[a > 0 ? "future" : "past"]; return "function" == typeof c ? c(b) : c.replace(/%s/i, b) }, ordinal: function (a) { return this._ordinal.replace("%d", a) }, _ordinal: "%d", preparse: function (a) { return a }, postformat: function (a) { return a }, week: function (a) { return $(a, this._week.dow, this._week.doy).week }, _week: { dow: 0, doy: 6 }, _invalidDate: "Invalid date", invalidDate: function () { return this._invalidDate } }), ib = function (c, d, e, f) { var g; return "boolean" == typeof e && (f = e, e = a), g = {}, g._isAMomentObject = !0, g._i = c, g._f = d, g._l = e, g._strict = f, g._isUTC = !1, g._pf = b(), ab(g) }, ib.suppressDeprecationWarnings = !1, ib.createFromInputFallback = c("moment construction falls back to js Date. This is discouraged and will be removed in upcoming major release. Please refer to https://github.com/moment/moment/issues/1407 for more info.", function (a) { a._d = new Date(a._i) }), ib.utc = function (c, d, e, f) { var g; return "boolean" == typeof e && (f = e, e = a), g = {}, g._isAMomentObject = !0, g._useUTC = !0, g._isUTC = !0, g._l = e, g._i = c, g._f = d, g._strict = f, g._pf = b(), ab(g).utc() }, ib.unix = function (a) { return ib(1e3 * a) }, ib.duration = function (a, b) { var c, d, e, f = a, g = null; return ib.isDuration(a) ? f = { ms: a._milliseconds, d: a._days, M: a._months } : "number" == typeof a ? (f = {}, b ? f[b] = a : f.milliseconds = a) : (g = zb.exec(a)) ? (c = "-" === g[1] ? -1 : 1, f = { y: 0, d: t(g[qb]) * c, h: t(g[rb]) * c, m: t(g[sb]) * c, s: t(g[tb]) * c, ms: t(g[ub]) * c }) : (g = Ab.exec(a)) && (c = "-" === g[1] ? -1 : 1, e = function (a) { var b = a && parseFloat(a.replace(",", ".")); return (isNaN(b) ? 0 : b) * c }, f = { y: e(g[2]), M: e(g[3]), d: e(g[4]), h: e(g[5]), m: e(g[6]), s: e(g[7]), w: e(g[8]) }), d = new h(f), ib.isDuration(a) && a.hasOwnProperty("_lang") && (d._lang = a._lang), d }, ib.version = lb, ib.defaultFormat = Ub, ib.momentProperties = wb, ib.updateOffset = function () { }, ib.lang = function (a, b) { var c; return a ? (b ? C(A(a), b) : null === b ? (D(a), a = "en") : vb[a] || E(a), c = ib.duration.fn._lang = ib.fn._lang = E(a), c._abbr) : ib.fn._lang._abbr }, ib.langData = function (a) { return a && a._lang && a._lang._abbr && (a = a._lang._abbr), E(a) }, ib.isMoment = function (a) { return a instanceof g || null != a && a.hasOwnProperty("_isAMomentObject") }, ib.isDuration = function (a) { return a instanceof h }, kb = dc.length - 1; kb >= 0; --kb) s(dc[kb]); ib.normalizeUnits = function (a) { return q(a) }, ib.invalid = function (a) { var b = ib.utc(0 / 0); return null != a ? i(b._pf, a) : b._pf.userInvalidated = !0, b }, ib.parseZone = function () { return ib.apply(null, arguments).parseZone() }, ib.parseTwoDigitYear = function (a) { return t(a) + (t(a) > 68 ? 1900 : 2e3) }, i(ib.fn = g.prototype, { clone: function () { return ib(this) }, valueOf: function () { return +this._d + 6e4 * (this._offset || 0) }, unix: function () { return Math.floor(+this / 1e3) }, toString: function () { return this.clone().lang("en").format("ddd MMM DD YYYY HH:mm:ss [GMT]ZZ") }, toDate: function () { return this._offset ? new Date(+this) : this._d }, toISOString: function () { var a = ib(this).utc(); return 0 < a.year() && a.year() <= 9999 ? H(a, "YYYY-MM-DD[T]HH:mm:ss.SSS[Z]") : H(a, "YYYYYY-MM-DD[T]HH:mm:ss.SSS[Z]") }, toArray: function () { var a = this; return [a.year(), a.month(), a.date(), a.hours(), a.minutes(), a.seconds(), a.milliseconds()] }, isValid: function () { return z(this) }, isDSTShifted: function () { return this._a ? this.isValid() && p(this._a, (this._isUTC ? ib.utc(this._a) : ib(this._a)).toArray()) > 0 : !1 }, parsingFlags: function () { return i({}, this._pf) }, invalidAt: function () { return this._pf.overflow }, utc: function () { return this.zone(0) }, local: function () { return this.zone(0), this._isUTC = !1, this }, format: function (a) { var b = H(this, a || ib.defaultFormat); return this.lang().postformat(b) }, add: function (a, b) { var c; return c = "string" == typeof a ? ib.duration(+b, a) : ib.duration(a, b), m(this, c, 1), this }, subtract: function (a, b) { var c; return c = "string" == typeof a ? ib.duration(+b, a) : ib.duration(a, b), m(this, c, -1), this }, diff: function (a, b, c) { var d, e, f = B(a, this), g = 6e4 * (this.zone() - f.zone()); return b = q(b), "year" === b || "month" === b ? (d = 432e5 * (this.daysInMonth() + f.daysInMonth()), e = 12 * (this.year() - f.year()) + (this.month() - f.month()), e += (this - ib(this).startOf("month") - (f - ib(f).startOf("month"))) / d, e -= 6e4 * (this.zone() - ib(this).startOf("month").zone() - (f.zone() - ib(f).startOf("month").zone())) / d, "year" === b && (e /= 12)) : (d = this - f, e = "second" === b ? d / 1e3 : "minute" === b ? d / 6e4 : "hour" === b ? d / 36e5 : "day" === b ? (d - g) / 864e5 : "week" === b ? (d - g) / 6048e5 : d), c ? e : k(e) }, from: function (a, b) { return ib.duration(this.diff(a)).lang(this.lang()._abbr).humanize(!b) }, fromNow: function (a) { return this.from(ib(), a) }, calendar: function () { var a = B(ib(), this).startOf("day"), b = this.diff(a, "days", !0), c = -6 > b ? "sameElse" : -1 > b ? "lastWeek" : 0 > b ? "lastDay" : 1 > b ? "sameDay" : 2 > b ? "nextDay" : 7 > b ? "nextWeek" : "sameElse"; return this.format(this.lang().calendar(c, this)) }, isLeapYear: function () { return x(this.year()) }, isDST: function () { return this.zone() < this.clone().month(0).zone() || this.zone() < this.clone().month(5).zone() }, day: function (a) { var b = this._isUTC ? this._d.getUTCDay() : this._d.getDay(); return null != a ? (a = X(a, this.lang()), this.add({ d: a - b })) : b }, month: eb("Month", !0), startOf: function (a) { switch (a = q(a)) { case "year": this.month(0); case "quarter": case "month": this.date(1); case "week": case "isoWeek": case "day": this.hours(0); case "hour": this.minutes(0); case "minute": this.seconds(0); case "second": this.milliseconds(0) } return "week" === a ? this.weekday(0) : "isoWeek" === a && this.isoWeekday(1), "quarter" === a && this.month(3 * Math.floor(this.month() / 3)), this }, endOf: function (a) { return a = q(a), this.startOf(a).add("isoWeek" === a ? "week" : a, 1).subtract("ms", 1) }, isAfter: function (a, b) { return b = "undefined" != typeof b ? b : "millisecond", +this.clone().startOf(b) > +ib(a).startOf(b) }, isBefore: function (a, b) { return b = "undefined" != typeof b ? b : "millisecond", +this.clone().startOf(b) < +ib(a).startOf(b) }, isSame: function (a, b) { return b = b || "ms", +this.clone().startOf(b) === +B(a, this).startOf(b) }, min: function (a) { return a = ib.apply(null, arguments), this > a ? this : a }, max: function (a) { return a = ib.apply(null, arguments), a > this ? this : a }, zone: function (a, b) { var c = this._offset || 0; return null == a ? this._isUTC ? c : this._d.getTimezoneOffset() : ("string" == typeof a && (a = K(a)), Math.abs(a) < 16 && (a = 60 * a), this._offset = a, this._isUTC = !0, c !== a && (!b || this._changeInProgress ? m(this, ib.duration(c - a, "m"), 1, !1) : this._changeInProgress || (this._changeInProgress = !0, ib.updateOffset(this, !0), this._changeInProgress = null)), this) }, zoneAbbr: function () { return this._isUTC ? "UTC" : "" }, zoneName: function () { return this._isUTC ? "Coordinated Universal Time" : "" }, parseZone: function () { return this._tzm ? this.zone(this._tzm) : "string" == typeof this._i && this.zone(this._i), this }, hasAlignedHourOffset: function (a) { return a = a ? ib(a).zone() : 0, (this.zone() - a) % 60 === 0 }, daysInMonth: function () { return u(this.year(), this.month()) }, dayOfYear: function (a) { var b = nb((ib(this).startOf("day") - ib(this).startOf("year")) / 864e5) + 1; return null == a ? b : this.add("d", a - b) }, quarter: function (a) { return null == a ? Math.ceil((this.month() + 1) / 3) : this.month(3 * (a - 1) + this.month() % 3) }, weekYear: function (a) { var b = $(this, this.lang()._week.dow, this.lang()._week.doy).year; return null == a ? b : this.add("y", a - b) }, isoWeekYear: function (a) { var b = $(this, 1, 4).year; return null == a ? b : this.add("y", a - b) }, week: function (a) { var b = this.lang().week(this); return null == a ? b : this.add("d", 7 * (a - b)) }, isoWeek: function (a) { var b = $(this, 1, 4).week; return null == a ? b : this.add("d", 7 * (a - b)) }, weekday: function (a) { var b = (this.day() + 7 - this.lang()._week.dow) % 7; return null == a ? b : this.add("d", a - b) }, isoWeekday: function (a) { return null == a ? this.day() || 7 : this.day(this.day() % 7 ? a : a - 7) }, isoWeeksInYear: function () { return v(this.year(), 1, 4) }, weeksInYear: function () { var a = this._lang._week; return v(this.year(), a.dow, a.doy) }, get: function (a) { return a = q(a), this[a]() }, set: function (a, b) { return a = q(a), "function" == typeof this[a] && this[a](b), this }, lang: function (b) { return b === a ? this._lang : (this._lang = E(b), this) } }), ib.fn.millisecond = ib.fn.milliseconds = eb("Milliseconds", !1), ib.fn.second = ib.fn.seconds = eb("Seconds", !1), ib.fn.minute = ib.fn.minutes = eb("Minutes", !1), ib.fn.hour = ib.fn.hours = eb("Hours", !0), ib.fn.date = eb("Date", !0), ib.fn.dates = c("dates accessor is deprecated. Use date instead.", eb("Date", !0)), ib.fn.year = eb("FullYear", !0), ib.fn.years = c("years accessor is deprecated. Use year instead.", eb("FullYear", !0)), ib.fn.days = ib.fn.day, ib.fn.months = ib.fn.month, ib.fn.weeks = ib.fn.week, ib.fn.isoWeeks = ib.fn.isoWeek, ib.fn.quarters = ib.fn.quarter, ib.fn.toJSON = ib.fn.toISOString, i(ib.duration.fn = h.prototype, { _bubble: function () { var a, b, c, d, e = this._milliseconds, f = this._days, g = this._months, h = this._data; h.milliseconds = e % 1e3, a = k(e / 1e3), h.seconds = a % 60, b = k(a / 60), h.minutes = b % 60, c = k(b / 60), h.hours = c % 24, f += k(c / 24), h.days = f % 30, g += k(f / 30), h.months = g % 12, d = k(g / 12), h.years = d }, weeks: function () { return k(this.days() / 7) }, valueOf: function () { return this._milliseconds + 864e5 * this._days + this._months % 12 * 2592e6 + 31536e6 * t(this._months / 12) }, humanize: function (a) { var b = +this, c = Z(b, !a, this.lang()); return a && (c = this.lang().pastFuture(b, c)), this.lang().postformat(c) }, add: function (a, b) { var c = ib.duration(a, b); return this._milliseconds += c._milliseconds, this._days += c._days, this._months += c._months, this._bubble(), this }, subtract: function (a, b) { var c = ib.duration(a, b); return this._milliseconds -= c._milliseconds, this._days -= c._days, this._months -= c._months, this._bubble(), this }, get: function (a) { return a = q(a), this[a.toLowerCase() + "s"]() }, as: function (a) { return a = q(a), this["as" + a.charAt(0).toUpperCase() + a.slice(1) + "s"]() }, lang: ib.fn.lang, toIsoString: function () { var a = Math.abs(this.years()), b = Math.abs(this.months()), c = Math.abs(this.days()), d = Math.abs(this.hours()), e = Math.abs(this.minutes()), f = Math.abs(this.seconds() + this.milliseconds() / 1e3); return this.asSeconds() ? (this.asSeconds() < 0 ? "-" : "") + "P" + (a ? a + "Y" : "") + (b ? b + "M" : "") + (c ? c + "D" : "") + (d || e || f ? "T" : "") + (d ? d + "H" : "") + (e ? e + "M" : "") + (f ? f + "S" : "") : "P0D" } }); for (kb in Yb) Yb.hasOwnProperty(kb) && (gb(kb, Yb[kb]), fb(kb.toLowerCase())); gb("Weeks", 6048e5), ib.duration.fn.asMonths = function () { return (+this - 31536e6 * this.years()) / 2592e6 + 12 * this.years() }, ib.lang("en", { ordinal: function (a) { var b = a % 10, c = 1 === t(a % 100 / 10) ? "th" : 1 === b ? "st" : 2 === b ? "nd" : 3 === b ? "rd" : "th"; return a + c } }), xb ? module.exports = ib : "function" == typeof define && define.amd ? (define("moment", function (a, b, c) { return c.config && c.config() && c.config().noGlobal === !0 && (mb.moment = jb), ib }), hb(!0)) : hb() }).call(this);

function addWeekdays(date, days) {
    date = moment(date); // use a clone
    while (days > 0) {
        date = date.add(1, 'days');
        // decrease "days" only if it's a weekday.
        if (date.isoWeekday() !== 6 && date.isoWeekday() !== 7) {
            days -= 1;
        }
    }
    return date;
}

/**
 * momentjs-business.js
 * businessDiff (mStartDate)
 * businessAdd (numberOfDays)
 */
(function () {
    var moment;
    moment = (typeof require !== "undefined" && require !== null) &&
             !require.amd ? require("moment") : this.moment;

    moment.fn.businessDiff = function (param) {
        param = moment(param);
        var signal = param.unix() < this.unix() ? 1 : -1;
        var start = moment.min(param, this).clone();
        var end = moment.max(param, this).clone();
        var start_offset = start.day() - 7;
        var end_offset = end.day();

        var end_sunday = end.clone().subtract('d', end_offset);
        var start_sunday = start.clone().subtract('d', start_offset);
        var weeks = end_sunday.diff(start_sunday, 'days') / 7;

        start_offset = Math.abs(start_offset);
        if (start_offset == 7)
            start_offset = 5;
        else if (start_offset == 1)
            start_offset = 0;
        else
            start_offset -= 2;


        if (end_offset == 6)
            end_offset--;

        return signal * (weeks * 5 + start_offset + end_offset);
    };

    moment.fn.businessAdd = function (days) {
        var signal = days < 0 ? -1 : 1;
        days = Math.abs(days);
        var d = this.clone().add(Math.floor(days / 5) * 7 * signal, 'd');
        var remaining = days % 5;
        while (remaining) {
            d.add(signal, 'd');
            if (d.day() !== 0 && d.day() !== 6)
                remaining--;
        }
        return d;
    };

    moment.fn.businessSubtract = function (days) {
        return this.businessAdd(-days);
    };

}).call(this);

var DateFormat = {};

(function ($) {
    var daysInWeek = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    var shortDaysInWeek = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    var shortMonthsInYear = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                                'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var longMonthsInYear = ['January', 'February', 'March', 'April', 'May', 'June',
                                'July', 'August', 'September', 'October', 'November', 'December'];
    var shortMonthsToNumber = {
        'Jan': '01', 'Feb': '02', 'Mar': '03', 'Apr': '04', 'May': '05', 'Jun': '06',
        'Jul': '07', 'Aug': '08', 'Sep': '09', 'Oct': '10', 'Nov': '11', 'Dec': '12'
    };

    var YYYYMMDD_MATCHER = /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.?\d{0,3}[Z\-+]?(\d{2}:?\d{2})?/;

    $.format = (function () {
        function numberToLongDay(value) {
            // 0 to Sunday
            // 1 to Monday
            return daysInWeek[parseInt(value, 10)] || value;
        }

        function numberToShortDay(value) {
            // 0 to Sun
            // 1 to Mon
            return shortDaysInWeek[parseInt(value, 10)] || value;
        }

        function numberToShortMonth(value) {
            // 1 to Jan
            // 2 to Feb
            var monthArrayIndex = parseInt(value, 10) - 1;
            return shortMonthsInYear[monthArrayIndex] || value;
        }

        function numberToLongMonth(value) {
            // 1 to January
            // 2 to February
            var monthArrayIndex = parseInt(value, 10) - 1;
            return longMonthsInYear[monthArrayIndex] || value;
        }

        function shortMonthToNumber(value) {
            // Jan to 01
            // Feb to 02
            return shortMonthsToNumber[value] || value;
        }

        function parseTime(value) {
            // 10:54:50.546
            // => hour: 10, minute: 54, second: 50, millis: 546
            // 10:54:50
            // => hour: 10, minute: 54, second: 50, millis: ''
            var time = value,
                values,
                subValues,
                hour,
                minute,
                second,
                millis = '',
                delimited,
                timeArray;

            if (time.indexOf('.') !== -1) {
                delimited = time.split('.');
                // split time and milliseconds
                time = delimited[0];
                millis = delimited[1];
            }

            timeArray = time.split(':');

            if (timeArray.length === 3) {
                hour = timeArray[0];
                minute = timeArray[1];
                // '20 GMT-0200 (BRST)'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 20
                // '20Z'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 20
                second = timeArray[2].replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // '01:10:20 GMT-0200 (BRST)'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 01:10:20
                // '01:10:20Z'.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                // => 01:10:20
                time = time.replace(/\s.+/, '').replace(/[a-z]/gi, '');
                return {
                    time: time,
                    hour: hour,
                    minute: minute,
                    second: second,
                    millis: millis
                };
            }

            return { time: '', hour: '', minute: '', second: '', millis: '' };
        }


        function padding(value, length) {
            var paddingCount = length - String(value).length;
            for (var i = 0; i < paddingCount; i++) {
                value = '0' + value;
            }
            return value;
        }

        return {

            parseDate: function (value) {
                var parsedDate = {
                    date: null,
                    year: null,
                    month: null,
                    dayOfMonth: null,
                    dayOfWeek: null,
                    time: null
                };

                if (typeof value == 'number') {
                    return this.parseDate(new Date(value));
                } else if (typeof value.getFullYear == 'function') {
                    parsedDate.year = String(value.getFullYear());
                    // d = new Date(1900, 1, 1) // 1 for Feb instead of Jan.
                    // => Thu Feb 01 1900 00:00:00
                    parsedDate.month = String(value.getMonth() + 1);
                    parsedDate.dayOfMonth = String(value.getDate());
                    parsedDate.time = parseTime(value.toTimeString());
                } else if (value.search(YYYYMMDD_MATCHER) != -1) {
                    /* 2009-04-19T16:11:05+02:00 || 2009-04-19T16:11:05Z */
                    values = value.split(/[T\+-]/);
                    parsedDate.year = values[0];
                    parsedDate.month = values[1];
                    parsedDate.dayOfMonth = values[2];
                    parsedDate.time = parseTime(values[3].split('.')[0]);
                } else {
                    values = value.split(' ');
                    if (values.length === 6 && isNaN(values[5])) {
                        // values[5] == year
                        /*
                         * This change is necessary to make `Mon Apr 28 2014 05:30:00 GMT-0300` work
                         * like `case 7`
                         * otherwise it will be considered like `Wed Jan 13 10:43:41 CET 2010
                         * Fixes: https://github.com/phstc/jquery-dateFormat/issues/64
                         */
                        values[values.length] = '()';
                    }
                    switch (values.length) {
                        case 6:
                            /* Wed Jan 13 10:43:41 CET 2010 */
                            parsedDate.year = values[5];
                            parsedDate.month = shortMonthToNumber(values[1]);
                            parsedDate.dayOfMonth = values[2];
                            parsedDate.time = parseTime(values[3]);
                            break;
                        case 2:
                            /* 2009-12-18 10:54:50.546 */
                            subValues = values[0].split('-');
                            parsedDate.year = subValues[0];
                            parsedDate.month = subValues[1];
                            parsedDate.dayOfMonth = subValues[2];
                            parsedDate.time = parseTime(values[1]);
                            break;
                        case 7:
                            /* Tue Mar 01 2011 12:01:42 GMT-0800 (PST) */
                        case 9:
                            /* added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0800 (China Standard Time) */
                        case 10:
                            /* added by Larry, for Fri Apr 08 2011 00:00:00 GMT+0200 (W. Europe Daylight Time) */
                            parsedDate.year = values[3];
                            parsedDate.month = shortMonthToNumber(values[1]);
                            parsedDate.dayOfMonth = values[2];
                            parsedDate.time = parseTime(values[4]);
                            break;
                        case 1:
                            /* added by Jonny, for 2012-02-07CET00:00:00 (Doctrine Entity -> Json Serializer) */
                            subValues = values[0].split('');
                            parsedDate.year = subValues[0] + subValues[1] + subValues[2] + subValues[3];
                            parsedDate.month = subValues[5] + subValues[6];
                            parsedDate.dayOfMonth = subValues[8] + subValues[9];
                            parsedDate.time = parseTime(subValues[13] + subValues[14] + subValues[15] + subValues[16] + subValues[17] + subValues[18] + subValues[19] + subValues[20]);
                            break;
                        default:
                            return null;
                    }
                }
                parsedDate.date = new Date(parsedDate.year, parsedDate.month - 1, parsedDate.dayOfMonth);
                parsedDate.dayOfWeek = String(parsedDate.date.getDay());

                return parsedDate;
            },

            date: function (value, format) {
                try {
                    var parsedDate = this.parseDate(value);

                    if (parsedDate === null) {
                        return value;
                    }

                    var date = parsedDate.date,
                        year = parsedDate.year,
                        month = parsedDate.month,
                        dayOfMonth = parsedDate.dayOfMonth,
                        dayOfWeek = parsedDate.dayOfWeek,
                        time = parsedDate.time;

                    var pattern = '',
                        retValue = '',
                        unparsedRest = '',
                        inQuote = false;

                    /* Issue 1 - variable scope issue in format.date (Thanks jakemonO) */
                    for (var i = 0; i < format.length; i++) {
                        var currentPattern = format.charAt(i);
                        // Look-Ahead Right (LALR)
                        var nextRight = format.charAt(i + 1);

                        if (inQuote) {
                            if (currentPattern == "'") {
                                retValue += (pattern === '') ? "'" : pattern;
                                pattern = '';
                                inQuote = false;
                            } else {
                                pattern += currentPattern;
                            }
                            continue;
                        }
                        pattern += currentPattern;
                        unparsedRest = '';
                        switch (pattern) {
                            case 'ddd':
                                retValue += numberToLongDay(dayOfWeek);
                                pattern = '';
                                break;
                            case 'dd':
                                if (nextRight === 'd') {
                                    break;
                                }
                                retValue += padding(dayOfMonth, 2);
                                pattern = '';
                                break;
                            case 'd':
                                if (nextRight === 'd') {
                                    break;
                                }
                                retValue += parseInt(dayOfMonth, 10);
                                pattern = '';
                                break;
                            case 'D':
                                if (dayOfMonth == 1 || dayOfMonth == 21 || dayOfMonth == 31) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'st';
                                } else if (dayOfMonth == 2 || dayOfMonth == 22) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'nd';
                                } else if (dayOfMonth == 3 || dayOfMonth == 23) {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'rd';
                                } else {
                                    dayOfMonth = parseInt(dayOfMonth, 10) + 'th';
                                }
                                retValue += dayOfMonth;
                                pattern = '';
                                break;
                            case 'MMMM':
                                retValue += numberToLongMonth(month);
                                pattern = '';
                                break;
                            case 'MMM':
                                if (nextRight === 'M') {
                                    break;
                                }
                                retValue += numberToShortMonth(month);
                                pattern = '';
                                break;
                            case 'MM':
                                if (nextRight === 'M') {
                                    break;
                                }
                                retValue += padding(month, 2);
                                pattern = '';
                                break;
                            case 'M':
                                if (nextRight === 'M') {
                                    break;
                                }
                                retValue += parseInt(month, 10);
                                pattern = '';
                                break;
                            case 'y':
                            case 'yyy':
                                if (nextRight === 'y') {
                                    break;
                                }
                                retValue += pattern;
                                pattern = '';
                                break;
                            case 'yy':
                                if (nextRight === 'y') {
                                    break;
                                }
                                retValue += String(year).slice(-2);
                                pattern = '';
                                break;
                            case 'yyyy':
                                retValue += year;
                                pattern = '';
                                break;
                            case 'HH':
                                retValue += padding(time.hour, 2);
                                pattern = '';
                                break;
                            case 'H':
                                if (nextRight === 'H') {
                                    break;
                                }
                                retValue += parseInt(time.hour, 10);
                                pattern = '';
                                break;
                            case 'hh':
                                /* time.hour is '00' as string == is used instead of === */
                                hour = (parseInt(time.hour, 10) === 0 ? 12 : time.hour < 13 ? time.hour
                                    : time.hour - 12);
                                retValue += padding(hour, 2);
                                pattern = '';
                                break;
                            case 'h':
                                if (nextRight === 'h') {
                                    break;
                                }
                                hour = (parseInt(time.hour, 10) === 0 ? 12 : time.hour < 13 ? time.hour
                                    : time.hour - 12);
                                retValue += parseInt(hour, 10);
                                // Fixing issue https://github.com/phstc/jquery-dateFormat/issues/21
                                // retValue = parseInt(retValue, 10);
                                pattern = '';
                                break;
                            case 'mm':
                                retValue += padding(time.minute, 2);
                                pattern = '';
                                break;
                            case 'm':
                                if (nextRight === 'm') {
                                    break;
                                }
                                retValue += time.minute;
                                pattern = '';
                                break;
                            case 'ss':
                                /* ensure only seconds are added to the return string */
                                retValue += padding(time.second.substring(0, 2), 2);
                                pattern = '';
                                break;
                            case 's':
                                if (nextRight === 's') {
                                    break;
                                }
                                retValue += time.second;
                                pattern = '';
                                break;
                            case 'S':
                            case 'SS':
                                if (nextRight === 'S') {
                                    break;
                                }
                                retValue += pattern;
                                pattern = '';
                                break;
                            case 'SSS':
                                retValue += time.millis.substring(0, 3);
                                pattern = '';
                                break;
                            case 'a':
                                retValue += time.hour >= 12 ? 'PM' : 'AM';
                                pattern = '';
                                break;
                            case 'p':
                                retValue += time.hour >= 12 ? 'p.m.' : 'a.m.';
                                pattern = '';
                                break;
                            case 'E':
                                retValue += numberToShortDay(dayOfWeek);
                                pattern = '';
                                break;
                            case "'":
                                pattern = '';
                                inQuote = true;
                                break;
                            default:
                                retValue += currentPattern;
                                pattern = '';
                                break;
                        }
                    }
                    retValue += unparsedRest;
                    return retValue;
                } catch (e) {
                    if (console && console.log) {
                        console.log(e);
                    }
                    return value;
                }
            },
            /*
             * JavaScript Pretty Date
             * Copyright (c) 2011 John Resig (ejohn.org)
             * Licensed under the MIT and GPL licenses.
             *
             * Takes an ISO time and returns a string representing how long ago the date
             * represents
             *
             * ('2008-01-28T20:24:17Z') // => '2 hours ago'
             * ('2008-01-27T22:24:17Z') // => 'Yesterday'
             * ('2008-01-26T22:24:17Z') // => '2 days ago'
             * ('2008-01-14T22:24:17Z') // => '2 weeks ago'
             * ('2007-12-15T22:24:17Z') // => 'more than 5 weeks ago'
             *
             */
            prettyDate: function (time) {
                var date;
                var diff;
                var day_diff;

                if (typeof time === 'string' || typeof time === 'number') {
                    date = new Date(time);
                }

                if (typeof time === 'object') {
                    date = new Date(time.toString());
                }

                diff = (((new Date()).getTime() - date.getTime()) / 1000);

                day_diff = Math.floor(diff / 86400);

                if (isNaN(day_diff) || day_diff < 0) {
                    return;
                }

                if (diff < 60) {
                    return 'just now';
                } else if (diff < 120) {
                    return '1 minute ago';
                } else if (diff < 3600) {
                    return Math.floor(diff / 60) + ' minutes ago';
                } else if (diff < 7200) {
                    return '1 hour ago';
                } else if (diff < 86400) {
                    return Math.floor(diff / 3600) + ' hours ago';
                } else if (day_diff === 1) {
                    return 'Yesterday';
                } else if (day_diff < 7) {
                    return day_diff + ' days ago';
                } else if (day_diff < 31) {
                    return Math.ceil(day_diff / 7) + ' weeks ago';
                } else if (day_diff >= 31) {
                    return 'more than 5 weeks ago';
                }
            },
            toBrowserTimeZone: function (value, format) {
                return this.date(new Date(value), format || 'MM/dd/yyyy HH:mm:ss');
            }
        };
    }());
}(DateFormat));
;// require dateFormat.js
// please check `dist/jquery.dateFormat.js` for a complete version
(function ($) {
    $.format = DateFormat.format;
}(jQuery));


/*
 * jQuery UI Timepicker
 *
 * Copyright 2010-2013, Francois Gelinas
 * Dual licensed under the MIT or GPL Version 2 licenses.
 * http://jquery.org/license
 *
 * http://fgelinas.com/code/timepicker
 *
 * Depends:
 *	jquery.ui.core.js
 *  jquery.ui.position.js (only if position settings are used)
 *
 * Change version 0.1.0 - moved the t-rex up here
 *
                                                  ____
       ___                                      .-~. /_"-._
      `-._~-.                                  / /_ "~o\  :Y
          \  \                                / : \~x.  ` ')
           ]  Y                              /  |  Y< ~-.__j
          /   !                        _.--~T : l  l<  /.-~
         /   /                 ____.--~ .   ` l /~\ \<|Y
        /   /             .-~~"        /| .    ',-~\ \L|
       /   /             /     .^   \ Y~Y \.^>/l_   "--'
      /   Y           .-"(  .  l__  j_j l_/ /~_.-~    .
     Y    l          /    \  )    ~~~." / `/"~ / \.__/l_
     |     \     _.-"      ~-{__     l  :  l._Z~-.___.--~
     |      ~---~           /   ~~"---\_  ' __[>
     l  .                _.^   ___     _>-y~
      \  \     .      .-~   .-~   ~>--"  /
       \  ~---"            /     ./  _.-'
        "-.,_____.,_  _.--~\     _.-~
                    ~~     (   _}       -Row
                           `. ~(
                             )  \
                            /,`--'~\--'~\
                  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                             ->T-Rex<-
*/

(function ($) {

    $.extend($.ui, { timepicker: { version: "0.3.3" } });

    var PROP_NAME = 'timepicker',
        tpuuid = new Date().getTime();

    /* Time picker manager.
    Use the singleton instance of this class, $.timepicker, to interact with the time picker.
    Settings for (groups of) time pickers are maintained in an instance object,
    allowing multiple different settings on the same page. */

    function Timepicker() {
        this.debug = true; // Change this to true to start debugging
        this._curInst = null; // The current instance in use
        this._disabledInputs = []; // List of time picker inputs that have been disabled
        this._timepickerShowing = false; // True if the popup picker is showing , false if not
        this._inDialog = false; // True if showing within a "dialog", false if not
        this._dialogClass = 'ui-timepicker-dialog'; // The name of the dialog marker class
        this._mainDivId = 'ui-timepicker-div'; // The ID of the main timepicker division
        this._inlineClass = 'ui-timepicker-inline'; // The name of the inline marker class
        this._currentClass = 'ui-timepicker-current'; // The name of the current hour / minutes marker class
        this._dayOverClass = 'ui-timepicker-days-cell-over'; // The name of the day hover marker class

        this.regional = []; // Available regional settings, indexed by language code
        this.regional[''] = { // Default regional settings
            hourText: 'Hour',           // Display text for hours section
            minuteText: 'Minute',       // Display text for minutes link
            amPmText: ['AM', 'PM'],     // Display text for AM PM
            closeButtonText: 'Done',        // Text for the confirmation button (ok button)
            nowButtonText: 'Now',           // Text for the now button
            deselectButtonText: 'Deselect'  // Text for the deselect button
        };
        this._defaults = { // Global defaults for all the time picker instances
            showOn: 'focus',    // 'focus' for popup on focus,
            // 'button' for trigger button, or 'both' for either (not yet implemented)
            button: null,                   // 'button' element that will trigger the timepicker
            showAnim: 'fadeIn',             // Name of jQuery animation for popup
            showOptions: {},                // Options for enhanced animations
            appendText: '',                 // Display text following the input box, e.g. showing the format

            beforeShow: null,               // Define a callback function executed before the timepicker is shown
            onSelect: null,                 // Define a callback function when a hour / minutes is selected
            onClose: null,                  // Define a callback function when the timepicker is closed

            timeSeparator: ':',             // The character to use to separate hours and minutes.
            periodSeparator: ' ',           // The character to use to separate the time from the time period.
            showPeriod: false,              // Define whether or not to show AM/PM with selected time
            showPeriodLabels: true,         // Show the AM/PM labels on the left of the time picker
            showLeadingZero: true,          // Define whether or not to show a leading zero for hours < 10. [true/false]
            showMinutesLeadingZero: true,   // Define whether or not to show a leading zero for minutes < 10.
            altField: '',                   // Selector for an alternate field to store selected time into
            defaultTime: 'now',             // Used as default time when input field is empty or for inline timePicker
            // (set to 'now' for the current time, '' for no highlighted time)
            myPosition: 'left top',         // Position of the dialog relative to the input.
            // see the position utility for more info : http://jqueryui.com/demos/position/
            atPosition: 'left bottom',      // Position of the input element to match
            // Note : if the position utility is not loaded, the timepicker will attach left top to left bottom
            //NEW: 2011-02-03
            onHourShow: null,			    // callback for enabling / disabling on selectable hours  ex : function(hour) { return true; }
            onMinuteShow: null,             // callback for enabling / disabling on time selection  ex : function(hour,minute) { return true; }

            hours: {
                starts: 0,                  // first displayed hour
                ends: 23                    // last displayed hour
            },
            minutes: {
                starts: 0,                  // first displayed minute
                ends: 55,                   // last displayed minute
                interval: 5,                // interval of displayed minutes
                manual: []                  // optional extra manual entries for minutes
            },
            rows: 4,                        // number of rows for the input tables, minimum 2, makes more sense if you use multiple of 2
            // 2011-08-05 0.2.4
            showHours: true,                // display the hours section of the dialog
            showMinutes: true,              // display the minute section of the dialog
            optionalMinutes: false,         // optionally parse inputs of whole hours with minutes omitted

            // buttons
            showCloseButton: false,         // shows an OK button to confirm the edit
            showNowButton: false,           // Shows the 'now' button
            showDeselectButton: false,       // Shows the deselect time button

            maxTime: {
                hour: null,
                minute: null
            },
            minTime: {
                hour: null,
                minute: null
            }

        };
        $.extend(this._defaults, this.regional['']);

        this.tpDiv = $('<div id="' + this._mainDivId + '" class="ui-timepicker ui-widget ui-helper-clearfix ui-corner-all " style="display: none"></div>');
    }

    $.extend(Timepicker.prototype, {
        /* Class name added to elements to indicate already configured with a time picker. */
        markerClassName: 'hasTimepicker',

        /* Debug logging (if enabled). */
        log: function () {
            if (this.debug)
                console.log.apply('', arguments);
        },

        _widgetTimepicker: function () {
            return this.tpDiv;
        },

        /* Override the default settings for all instances of the time picker.
        @param  settings  object - the new settings to use as defaults (anonymous object)
        @return the manager object */
        setDefaults: function (settings) {
            extendRemove(this._defaults, settings || {});
            return this;
        },

        /* Attach the time picker to a jQuery selection.
        @param  target    element - the target input field or division or span
        @param  settings  object - the new settings to use for this time picker instance (anonymous) */
        _attachTimepicker: function (target, settings) {
            // check for settings on the control itself - in namespace 'time:'
            var inlineSettings = null;
            for (var attrName in this._defaults) {
                var attrValue = target.getAttribute('time:' + attrName);
                if (attrValue) {
                    inlineSettings = inlineSettings || {};
                    try {
                        inlineSettings[attrName] = eval(attrValue);
                    } catch (err) {
                        inlineSettings[attrName] = attrValue;
                    }
                }
            }
            var nodeName = target.nodeName.toLowerCase();
            var inline = (nodeName == 'div' || nodeName == 'span');

            if (!target.id) {
                this.uuid += 1;
                target.id = 'tp' + this.uuid;
            }
            var inst = this._newInst($(target), inline);
            inst.settings = $.extend({}, settings || {}, inlineSettings || {});
            if (nodeName == 'input') {
                this._connectTimepicker(target, inst);
                // init inst.hours and inst.minutes from the input value
                this._setTimeFromField(inst);
            } else if (inline) {
                this._inlineTimepicker(target, inst);
            }


        },

        /* Create a new instance object. */
        _newInst: function (target, inline) {
            var id = target[0].id.replace(/([^A-Za-z0-9_-])/g, '\\\\$1'); // escape jQuery meta chars
            return {
                id: id, input: target, // associated target
                inline: inline, // is timepicker inline or not :
                tpDiv: (!inline ? this.tpDiv : // presentation div
                    $('<div class="' + this._inlineClass + ' ui-timepicker ui-widget  ui-helper-clearfix"></div>'))
            };
        },

        /* Attach the time picker to an input field. */
        _connectTimepicker: function (target, inst) {
            var input = $(target);
            inst.append = $([]);
            inst.trigger = $([]);
            if (input.hasClass(this.markerClassName)) { return; }
            this._attachments(input, inst);
            input.addClass(this.markerClassName).
                keydown(this._doKeyDown).
                keyup(this._doKeyUp).
                bind("setData.timepicker", function (event, key, value) {
                    inst.settings[key] = value;
                }).
                bind("getData.timepicker", function (event, key) {
                    return this._get(inst, key);
                });
            $.data(target, PROP_NAME, inst);
        },

        /* Handle keystrokes. */
        _doKeyDown: function (event) {
            var inst = $.timepicker._getInst(event.target);
            var handled = true;
            inst._keyEvent = true;
            if ($.timepicker._timepickerShowing) {
                switch (event.keyCode) {
                    case 9: $.timepicker._hideTimepicker();
                        handled = false;
                        break; // hide on tab out
                    case 13:
                        $.timepicker._updateSelectedValue(inst);
                        $.timepicker._hideTimepicker();

                        return false; // don't submit the form
                        break; // select the value on enter
                    case 27: $.timepicker._hideTimepicker();
                        break; // hide on escape
                    default: handled = false;
                }
            }
            else if (event.keyCode == 36 && event.ctrlKey) { // display the time picker on ctrl+home
                $.timepicker._showTimepicker(this);
            }
            else {
                handled = false;
            }
            if (handled) {
                event.preventDefault();
                event.stopPropagation();
            }
        },

        /* Update selected time on keyUp */
        /* Added verion 0.0.5 */
        _doKeyUp: function (event) {
            var inst = $.timepicker._getInst(event.target);
            $.timepicker._setTimeFromField(inst);
            $.timepicker._updateTimepicker(inst);
        },

        /* Make attachments based on settings. */
        _attachments: function (input, inst) {
            var appendText = this._get(inst, 'appendText');
            var isRTL = this._get(inst, 'isRTL');
            if (inst.append) { inst.append.remove(); }
            if (appendText) {
                inst.append = $('<span class="' + this._appendClass + '">' + appendText + '</span>');
                input[isRTL ? 'before' : 'after'](inst.append);
            }
            input.unbind('focus.timepicker', this._showTimepicker);
            input.unbind('click.timepicker', this._adjustZIndex);

            if (inst.trigger) { inst.trigger.remove(); }

            var showOn = this._get(inst, 'showOn');
            if (showOn == 'focus' || showOn == 'both') { // pop-up time picker when in the marked field
                input.bind("focus.timepicker", this._showTimepicker);
                input.bind("click.timepicker", this._adjustZIndex);
            }
            if (showOn == 'button' || showOn == 'both') { // pop-up time picker when 'button' element is clicked
                var button = this._get(inst, 'button');

                // Add button if button element is not set
                if (button == null) {
                    button = $('<button class="ui-timepicker-trigger" type="button">...</button>');
                    input.after(button);
                }

                $(button).bind("click.timepicker", function () {
                    if ($.timepicker._timepickerShowing && $.timepicker._lastInput == input[0]) {
                        $.timepicker._hideTimepicker();
                    } else if (!inst.input.is(':disabled')) {
                        $.timepicker._showTimepicker(input[0]);
                    }
                    return false;
                });

            }
        },


        /* Attach an inline time picker to a div. */
        _inlineTimepicker: function (target, inst) {
            var divSpan = $(target);
            if (divSpan.hasClass(this.markerClassName))
                return;
            divSpan.addClass(this.markerClassName).append(inst.tpDiv).
                bind("setData.timepicker", function (event, key, value) {
                    inst.settings[key] = value;
                }).bind("getData.timepicker", function (event, key) {
                    return this._get(inst, key);
                });
            $.data(target, PROP_NAME, inst);

            this._setTimeFromField(inst);
            this._updateTimepicker(inst);
            inst.tpDiv.show();
        },

        _adjustZIndex: function (input) {
            input = input.target || input;
            var inst = $.timepicker._getInst(input);
            inst.tpDiv.css('zIndex', $.timepicker._getZIndex(input) + 1);
        },

        /* Pop-up the time picker for a given input field.
        @param  input  element - the input field attached to the time picker or
        event - if triggered by focus */
        _showTimepicker: function (input) {
            input = input.target || input;
            if (input.nodeName.toLowerCase() != 'input') { input = $('input', input.parentNode)[0]; } // find from button/image trigger

            if ($.timepicker._isDisabledTimepicker(input) || $.timepicker._lastInput == input) { return; } // already here

            // fix v 0.0.8 - close current timepicker before showing another one
            $.timepicker._hideTimepicker();

            var inst = $.timepicker._getInst(input);
            if ($.timepicker._curInst && $.timepicker._curInst != inst) {
                $.timepicker._curInst.tpDiv.stop(true, true);
            }
            var beforeShow = $.timepicker._get(inst, 'beforeShow');
            extendRemove(inst.settings, (beforeShow ? beforeShow.apply(input, [input, inst]) : {}));
            inst.lastVal = null;
            $.timepicker._lastInput = input;

            $.timepicker._setTimeFromField(inst);

            // calculate default position
            if ($.timepicker._inDialog) { input.value = ''; } // hide cursor
            if (!$.timepicker._pos) { // position below input
                $.timepicker._pos = $.timepicker._findPos(input);
                $.timepicker._pos[1] += input.offsetHeight; // add the height
            }
            var isFixed = false;
            $(input).parents().each(function () {
                isFixed |= $(this).css('position') == 'fixed';
                return !isFixed;
            });

            var offset = { left: $.timepicker._pos[0], top: $.timepicker._pos[1] };

            $.timepicker._pos = null;
            // determine sizing offscreen
            inst.tpDiv.css({ position: 'absolute', display: 'block', top: '-1000px' });
            $.timepicker._updateTimepicker(inst);


            // position with the ui position utility, if loaded
            if ((!inst.inline) && (typeof $.ui.position == 'object')) {
                inst.tpDiv.position({
                    of: inst.input,
                    my: $.timepicker._get(inst, 'myPosition'),
                    at: $.timepicker._get(inst, 'atPosition'),
                    // offset: $( "#offset" ).val(),
                    // using: using,
                    collision: 'flip'
                });
                var offset = inst.tpDiv.offset();
                $.timepicker._pos = [offset.top, offset.left];
            }


            // reset clicked state
            inst._hoursClicked = false;
            inst._minutesClicked = false;

            // fix width for dynamic number of time pickers
            // and adjust position before showing
            offset = $.timepicker._checkOffset(inst, offset, isFixed);
            inst.tpDiv.css({
                position: ($.timepicker._inDialog && $.blockUI ?
                    'static' : (isFixed ? 'fixed' : 'absolute')), display: 'none',
                left: offset.left + 'px', top: offset.top + 'px'
            });
            if (!inst.inline) {
                var showAnim = $.timepicker._get(inst, 'showAnim');
                var duration = $.timepicker._get(inst, 'duration');

                var postProcess = function () {
                    $.timepicker._timepickerShowing = true;
                    var borders = $.timepicker._getBorders(inst.tpDiv);
                    inst.tpDiv.find('iframe.ui-timepicker-cover'). // IE6- only
					css({
					    left: -borders[0], top: -borders[1],
					    width: inst.tpDiv.outerWidth(), height: inst.tpDiv.outerHeight()
					});
                };

                // Fixed the zIndex problem for real (I hope) - FG - v 0.2.9
                $.timepicker._adjustZIndex(input);
                //inst.tpDiv.css('zIndex', $.timepicker._getZIndex(input) +1);

                if ($.effects && $.effects[showAnim]) {
                    inst.tpDiv.show(showAnim, $.timepicker._get(inst, 'showOptions'), duration, postProcess);
                }
                else {
                    inst.tpDiv.show((showAnim ? duration : null), postProcess);
                }
                if (!showAnim || !duration) { postProcess(); }
                if (inst.input.is(':visible') && !inst.input.is(':disabled')) { inst.input.focus(); }
                $.timepicker._curInst = inst;
            }
        },

        // This is an enhanced copy of the zIndex function of UI core 1.8.?? For backward compatibility.
        // Enhancement returns maximum zindex value discovered while traversing parent elements,
        // rather than the first zindex value found. Ensures the timepicker popup will be in front,
        // even in funky scenarios like non-jq dialog containers with large fixed zindex values and
        // nested zindex-influenced elements of their own.
        _getZIndex: function (target) {
            var elem = $(target);
            var maxValue = 0;
            var position, value;
            while (elem.length && elem[0] !== document) {
                position = elem.css("position");
                if (position === "absolute" || position === "relative" || position === "fixed") {
                    value = parseInt(elem.css("zIndex"), 10);
                    if (!isNaN(value) && value !== 0) {
                        if (value > maxValue) { maxValue = value; }
                    }
                }
                elem = elem.parent();
            }

            return maxValue;
        },

        /* Refresh the time picker
           @param   target  element - The target input field or inline container element. */
        _refreshTimepicker: function (target) {
            var inst = this._getInst(target);
            if (inst) {
                this._updateTimepicker(inst);
            }
        },


        /* Generate the time picker content. */
        _updateTimepicker: function (inst) {
            inst.tpDiv.empty().append(this._generateHTML(inst));
            this._rebindDialogEvents(inst);

        },

        _rebindDialogEvents: function (inst) {
            var borders = $.timepicker._getBorders(inst.tpDiv),
                self = this;
            inst.tpDiv
			.find('iframe.ui-timepicker-cover') // IE6- only
				.css({
				    left: -borders[0], top: -borders[1],
				    width: inst.tpDiv.outerWidth(), height: inst.tpDiv.outerHeight()
				})
			.end()
            // after the picker html is appended bind the click & double click events (faster in IE this way
            // then letting the browser interpret the inline events)
            // the binding for the minute cells also exists in _updateMinuteDisplay
            .find('.ui-timepicker-minute-cell')
                .unbind()
                .bind("click", { fromDoubleClick: false }, $.proxy($.timepicker.selectMinutes, this))
                .bind("dblclick", { fromDoubleClick: true }, $.proxy($.timepicker.selectMinutes, this))
            .end()
            .find('.ui-timepicker-hour-cell')
                .unbind()
                .bind("click", { fromDoubleClick: false }, $.proxy($.timepicker.selectHours, this))
                .bind("dblclick", { fromDoubleClick: true }, $.proxy($.timepicker.selectHours, this))
            .end()
			.find('.ui-timepicker td a')
                .unbind()
				.bind('mouseout', function () {
				    $(this).removeClass('ui-state-hover');
				    if (this.className.indexOf('ui-timepicker-prev') != -1) $(this).removeClass('ui-timepicker-prev-hover');
				    if (this.className.indexOf('ui-timepicker-next') != -1) $(this).removeClass('ui-timepicker-next-hover');
				})
				.bind('mouseover', function () {
				    if (!self._isDisabledTimepicker(inst.inline ? inst.tpDiv.parent()[0] : inst.input[0])) {
				        $(this).parents('.ui-timepicker-calendar').find('a').removeClass('ui-state-hover');
				        $(this).addClass('ui-state-hover');
				        if (this.className.indexOf('ui-timepicker-prev') != -1) $(this).addClass('ui-timepicker-prev-hover');
				        if (this.className.indexOf('ui-timepicker-next') != -1) $(this).addClass('ui-timepicker-next-hover');
				    }
				})
			.end()
			.find('.' + this._dayOverClass + ' a')
				.trigger('mouseover')
			.end()
            .find('.ui-timepicker-now').bind("click", function (e) {
                $.timepicker.selectNow(e);
            }).end()
            .find('.ui-timepicker-deselect').bind("click", function (e) {
                $.timepicker.deselectTime(e);
            }).end()
            .find('.ui-timepicker-close').bind("click", function (e) {
                $.timepicker._hideTimepicker();
            }).end();
        },

        /* Generate the HTML for the current state of the time picker. */
        _generateHTML: function (inst) {

            var h, m, row, col, html, hoursHtml, minutesHtml = '',
                showPeriod = (this._get(inst, 'showPeriod') == true),
                showPeriodLabels = (this._get(inst, 'showPeriodLabels') == true),
                showLeadingZero = (this._get(inst, 'showLeadingZero') == true),
                showHours = (this._get(inst, 'showHours') == true),
                showMinutes = (this._get(inst, 'showMinutes') == true),
                amPmText = this._get(inst, 'amPmText'),
                rows = this._get(inst, 'rows'),
                amRows = 0,
                pmRows = 0,
                amItems = 0,
                pmItems = 0,
                amFirstRow = 0,
                pmFirstRow = 0,
                hours = Array(),
                hours_options = this._get(inst, 'hours'),
                hoursPerRow = null,
                hourCounter = 0,
                hourLabel = this._get(inst, 'hourText'),
                showCloseButton = this._get(inst, 'showCloseButton'),
                closeButtonText = this._get(inst, 'closeButtonText'),
                showNowButton = this._get(inst, 'showNowButton'),
                nowButtonText = this._get(inst, 'nowButtonText'),
                showDeselectButton = this._get(inst, 'showDeselectButton'),
                deselectButtonText = this._get(inst, 'deselectButtonText'),
                showButtonPanel = showCloseButton || showNowButton || showDeselectButton;



            // prepare all hours and minutes, makes it easier to distribute by rows
            for (h = hours_options.starts; h <= hours_options.ends; h++) {
                hours.push(h);
            }
            hoursPerRow = Math.ceil(hours.length / rows); // always round up

            if (showPeriodLabels) {
                for (hourCounter = 0; hourCounter < hours.length; hourCounter++) {
                    if (hours[hourCounter] < 12) {
                        amItems++;
                    }
                    else {
                        pmItems++;
                    }
                }
                hourCounter = 0;

                amRows = Math.floor(amItems / hours.length * rows);
                pmRows = Math.floor(pmItems / hours.length * rows);

                // assign the extra row to the period that is more densely populated
                if (rows != amRows + pmRows) {
                    // Make sure: AM Has Items and either PM Does Not, AM has no rows yet, or AM is more dense
                    if (amItems && (!pmItems || !amRows || (pmRows && amItems / amRows >= pmItems / pmRows))) {
                        amRows++;
                    } else {
                        pmRows++;
                    }
                }
                amFirstRow = Math.min(amRows, 1);
                pmFirstRow = amRows + 1;

                if (amRows == 0) {
                    hoursPerRow = Math.ceil(pmItems / pmRows);
                } else if (pmRows == 0) {
                    hoursPerRow = Math.ceil(amItems / amRows);
                } else {
                    hoursPerRow = Math.ceil(Math.max(amItems / amRows, pmItems / pmRows));
                }
            }


            html = '<table class="ui-timepicker-table ui-widget-content ui-corner-all"><tr>';

            if (showHours) {

                html += '<td class="ui-timepicker-hours">' +
                        '<div class="ui-timepicker-title ui-widget-header ui-helper-clearfix ui-corner-all">' +
                        hourLabel +
                        '</div>' +
                        '<table class="ui-timepicker">';

                for (row = 1; row <= rows; row++) {
                    html += '<tr>';
                    // AM
                    if (row == amFirstRow && showPeriodLabels) {
                        html += '<th rowspan="' + amRows.toString() + '" class="periods" scope="row">' + amPmText[0] + '</th>';
                    }
                    // PM
                    if (row == pmFirstRow && showPeriodLabels) {
                        html += '<th rowspan="' + pmRows.toString() + '" class="periods" scope="row">' + amPmText[1] + '</th>';
                    }
                    for (col = 1; col <= hoursPerRow; col++) {
                        if (showPeriodLabels && row < pmFirstRow && hours[hourCounter] >= 12) {
                            html += this._generateHTMLHourCell(inst, undefined, showPeriod, showLeadingZero);
                        } else {
                            html += this._generateHTMLHourCell(inst, hours[hourCounter], showPeriod, showLeadingZero);
                            hourCounter++;
                        }
                    }
                    html += '</tr>';
                }
                html += '</table>' + // Close the hours cells table
                        '</td>'; // Close the Hour td
            }

            if (showMinutes) {
                html += '<td class="ui-timepicker-minutes">';
                html += this._generateHTMLMinutes(inst);
                html += '</td>';
            }

            html += '</tr>';


            if (showButtonPanel) {
                var buttonPanel = '<tr><td colspan="3"><div class="ui-timepicker-buttonpane ui-widget-content">';
                if (showNowButton) {
                    buttonPanel += '<button type="button" class="ui-timepicker-now ui-state-default ui-corner-all" '
                                   + ' data-timepicker-instance-id="#' + inst.id.replace(/\\\\/g, "\\") + '" >'
                                   + nowButtonText + '</button>';
                }
                if (showDeselectButton) {
                    buttonPanel += '<button type="button" class="ui-timepicker-deselect ui-state-default ui-corner-all" '
                                   + ' data-timepicker-instance-id="#' + inst.id.replace(/\\\\/g, "\\") + '" >'
                                   + deselectButtonText + '</button>';
                }
                if (showCloseButton) {
                    buttonPanel += '<button type="button" class="ui-timepicker-close ui-state-default ui-corner-all" '
                                   + ' data-timepicker-instance-id="#' + inst.id.replace(/\\\\/g, "\\") + '" >'
                                   + closeButtonText + '</button>';
                }

                html += buttonPanel + '</div></td></tr>';
            }
            html += '</table>';

            return html;
        },

        /* Special function that update the minutes selection in currently visible timepicker
         * called on hour selection when onMinuteShow is defined  */
        _updateMinuteDisplay: function (inst) {
            var newHtml = this._generateHTMLMinutes(inst);
            inst.tpDiv.find('td.ui-timepicker-minutes').html(newHtml);
            this._rebindDialogEvents(inst);
            // after the picker html is appended bind the click & double click events (faster in IE this way
            // then letting the browser interpret the inline events)
            // yes I know, duplicate code, sorry
            /*                .find('.ui-timepicker-minute-cell')
                                .bind("click", { fromDoubleClick:false }, $.proxy($.timepicker.selectMinutes, this))
                                .bind("dblclick", { fromDoubleClick:true }, $.proxy($.timepicker.selectMinutes, this));
            */

        },

        /*
         * Generate the minutes table
         * This is separated from the _generateHTML function because is can be called separately (when hours changes)
         */
        _generateHTMLMinutes: function (inst) {

            var m, row, html = '',
                rows = this._get(inst, 'rows'),
                minutes = Array(),
                minutes_options = this._get(inst, 'minutes'),
                minutesPerRow = null,
                minuteCounter = 0,
                showMinutesLeadingZero = (this._get(inst, 'showMinutesLeadingZero') == true),
                onMinuteShow = this._get(inst, 'onMinuteShow'),
                minuteLabel = this._get(inst, 'minuteText');

            if (!minutes_options.starts) {
                minutes_options.starts = 0;
            }
            if (!minutes_options.ends) {
                minutes_options.ends = 59;
            }
            if (!minutes_options.manual) {
                minutes_options.manual = [];
            }
            for (m = minutes_options.starts; m <= minutes_options.ends; m += minutes_options.interval) {
                minutes.push(m);
            }
            for (i = 0; i < minutes_options.manual.length; i++) {
                var currMin = minutes_options.manual[i];

                // Validate & filter duplicates of manual minute input
                if (typeof currMin != 'number' || currMin < 0 || currMin > 59 || $.inArray(currMin, minutes) >= 0) {
                    continue;
                }
                minutes.push(currMin);
            }

            // Sort to get correct order after adding manual minutes
            // Use compare function to sort by number, instead of string (default)
            minutes.sort(function (a, b) {
                return a - b;
            });

            minutesPerRow = Math.round(minutes.length / rows + 0.49); // always round up

            /*
             * The minutes table
             */
            // if currently selected minute is not enabled, we have a problem and need to select a new minute.
            if (onMinuteShow &&
                (onMinuteShow.apply((inst.input ? inst.input[0] : null), [inst.hours, inst.minutes]) == false)) {
                // loop minutes and select first available
                for (minuteCounter = 0; minuteCounter < minutes.length; minuteCounter += 1) {
                    m = minutes[minuteCounter];
                    if (onMinuteShow.apply((inst.input ? inst.input[0] : null), [inst.hours, m])) {
                        inst.minutes = m;
                        break;
                    }
                }
            }



            html += '<div class="ui-timepicker-title ui-widget-header ui-helper-clearfix ui-corner-all">' +
                    minuteLabel +
                    '</div>' +
                    '<table class="ui-timepicker">';

            minuteCounter = 0;
            for (row = 1; row <= rows; row++) {
                html += '<tr>';
                while (minuteCounter < row * minutesPerRow) {
                    var m = minutes[minuteCounter];
                    var displayText = '';
                    if (m !== undefined) {
                        displayText = (m < 10) && showMinutesLeadingZero ? "0" + m.toString() : m.toString();
                    }
                    html += this._generateHTMLMinuteCell(inst, m, displayText);
                    minuteCounter++;
                }
                html += '</tr>';
            }

            html += '</table>';

            return html;
        },

        /* Generate the content of a "Hour" cell */
        _generateHTMLHourCell: function (inst, hour, showPeriod, showLeadingZero) {

            var displayHour = hour;
            if ((hour > 12) && showPeriod) {
                displayHour = hour - 12;
            }
            if ((displayHour == 0) && showPeriod) {
                displayHour = 12;
            }
            if ((displayHour < 10) && showLeadingZero) {
                displayHour = '0' + displayHour;
            }

            var html = "";
            var enabled = true;
            var onHourShow = this._get(inst, 'onHourShow');		//custom callback
            var maxTime = this._get(inst, 'maxTime');
            var minTime = this._get(inst, 'minTime');

            if (hour == undefined) {
                html = '<td><span class="ui-state-default ui-state-disabled">&nbsp;</span></td>';
                return html;
            }

            if (onHourShow) {
                enabled = onHourShow.apply((inst.input ? inst.input[0] : null), [hour]);
            }

            if (enabled) {
                if (!isNaN(parseInt(maxTime.hour)) && hour > maxTime.hour) enabled = false;
                if (!isNaN(parseInt(minTime.hour)) && hour < minTime.hour) enabled = false;
            }

            if (enabled) {
                html = '<td class="ui-timepicker-hour-cell" data-timepicker-instance-id="#' + inst.id.replace(/\\\\/g, "\\") + '" data-hour="' + hour.toString() + '">' +
                   '<a class="ui-state-default ' +
                   (hour == inst.hours ? 'ui-state-active' : '') +
                   '">' +
                   displayHour.toString() +
                   '</a></td>';
            }
            else {
                html =
            		'<td>' +
		                '<span class="ui-state-default ui-state-disabled ' +
		                (hour == inst.hours ? ' ui-state-active ' : ' ') +
		                '">' +
		                displayHour.toString() +
		                '</span>' +
		            '</td>';
            }
            return html;
        },

        /* Generate the content of a "Hour" cell */
        _generateHTMLMinuteCell: function (inst, minute, displayText) {
            var html = "";
            var enabled = true;
            var hour = inst.hours;
            var onMinuteShow = this._get(inst, 'onMinuteShow');		//custom callback
            var maxTime = this._get(inst, 'maxTime');
            var minTime = this._get(inst, 'minTime');

            if (onMinuteShow) {
                //NEW: 2011-02-03  we should give the hour as a parameter as well!
                enabled = onMinuteShow.apply((inst.input ? inst.input[0] : null), [inst.hours, minute]);		//trigger callback
            }

            if (minute == undefined) {
                html = '<td><span class="ui-state-default ui-state-disabled">&nbsp;</span></td>';
                return html;
            }

            if (enabled && hour !== null) {
                if (!isNaN(parseInt(maxTime.hour)) && !isNaN(parseInt(maxTime.minute)) && hour >= maxTime.hour && minute > maxTime.minute) enabled = false;
                if (!isNaN(parseInt(minTime.hour)) && !isNaN(parseInt(minTime.minute)) && hour <= minTime.hour && minute < minTime.minute) enabled = false;
            }

            if (enabled) {
                html = '<td class="ui-timepicker-minute-cell" data-timepicker-instance-id="#' + inst.id.replace(/\\\\/g, "\\") + '" data-minute="' + minute.toString() + '" >' +
                      '<a class="ui-state-default ' +
                      (minute == inst.minutes ? 'ui-state-active' : '') +
                      '" >' +
                      displayText +
                      '</a></td>';
            }
            else {

                html = '<td>' +
	                 '<span class="ui-state-default ui-state-disabled" >' +
	                 	displayText +
	                 '</span>' +
                 '</td>';
            }
            return html;
        },


        /* Detach a timepicker from its control.
           @param  target    element - the target input field or division or span */
        _destroyTimepicker: function (target) {
            var $target = $(target);
            var inst = $.data(target, PROP_NAME);
            if (!$target.hasClass(this.markerClassName)) {
                return;
            }
            var nodeName = target.nodeName.toLowerCase();
            $.removeData(target, PROP_NAME);
            if (nodeName == 'input') {
                inst.append.remove();
                inst.trigger.remove();
                $target.removeClass(this.markerClassName)
                    .unbind('focus.timepicker', this._showTimepicker)
                    .unbind('click.timepicker', this._adjustZIndex);
            } else if (nodeName == 'div' || nodeName == 'span')
                $target.removeClass(this.markerClassName).empty();
        },

        /* Enable the date picker to a jQuery selection.
           @param  target    element - the target input field or division or span */
        _enableTimepicker: function (target) {
            var $target = $(target),
                target_id = $target.attr('id'),
                inst = $.data(target, PROP_NAME);

            if (!$target.hasClass(this.markerClassName)) {
                return;
            }
            var nodeName = target.nodeName.toLowerCase();
            if (nodeName == 'input') {
                target.disabled = false;
                var button = this._get(inst, 'button');
                $(button).removeClass('ui-state-disabled').disabled = false;
                inst.trigger.filter('button').
                    each(function () { this.disabled = false; }).end();
            }
            else if (nodeName == 'div' || nodeName == 'span') {
                var inline = $target.children('.' + this._inlineClass);
                inline.children().removeClass('ui-state-disabled');
                inline.find('button').each(
                    function () { this.disabled = false }
                )
            }
            this._disabledInputs = $.map(this._disabledInputs,
                function (value) { return (value == target_id ? null : value); }); // delete entry
        },

        /* Disable the time picker to a jQuery selection.
           @param  target    element - the target input field or division or span */
        _disableTimepicker: function (target) {
            var $target = $(target);
            var inst = $.data(target, PROP_NAME);
            if (!$target.hasClass(this.markerClassName)) {
                return;
            }
            var nodeName = target.nodeName.toLowerCase();
            if (nodeName == 'input') {
                var button = this._get(inst, 'button');

                $(button).addClass('ui-state-disabled').disabled = true;
                target.disabled = true;

                inst.trigger.filter('button').
                    each(function () { this.disabled = true; }).end();

            }
            else if (nodeName == 'div' || nodeName == 'span') {
                var inline = $target.children('.' + this._inlineClass);
                inline.children().addClass('ui-state-disabled');
                inline.find('button').each(
                    function () { this.disabled = true }
                )

            }
            this._disabledInputs = $.map(this._disabledInputs,
                function (value) { return (value == target ? null : value); }); // delete entry
            this._disabledInputs[this._disabledInputs.length] = $target.attr('id');
        },

        /* Is the first field in a jQuery collection disabled as a timepicker?
        @param  target_id element - the target input field or division or span
        @return boolean - true if disabled, false if enabled */
        _isDisabledTimepicker: function (target_id) {
            if (!target_id) { return false; }
            for (var i = 0; i < this._disabledInputs.length; i++) {
                if (this._disabledInputs[i] == target_id) { return true; }
            }
            return false;
        },

        /* Check positioning to remain on screen. */
        _checkOffset: function (inst, offset, isFixed) {
            var tpWidth = inst.tpDiv.outerWidth();
            var tpHeight = inst.tpDiv.outerHeight();
            var inputWidth = inst.input ? inst.input.outerWidth() : 0;
            var inputHeight = inst.input ? inst.input.outerHeight() : 0;
            var viewWidth = document.documentElement.clientWidth + $(document).scrollLeft();
            var viewHeight = document.documentElement.clientHeight + $(document).scrollTop();

            offset.left -= (this._get(inst, 'isRTL') ? (tpWidth - inputWidth) : 0);
            offset.left -= (isFixed && offset.left == inst.input.offset().left) ? $(document).scrollLeft() : 0;
            offset.top -= (isFixed && offset.top == (inst.input.offset().top + inputHeight)) ? $(document).scrollTop() : 0;

            // now check if timepicker is showing outside window viewport - move to a better place if so.
            offset.left -= Math.min(offset.left, (offset.left + tpWidth > viewWidth && viewWidth > tpWidth) ?
			Math.abs(offset.left + tpWidth - viewWidth) : 0);
            offset.top -= Math.min(offset.top, (offset.top + tpHeight > viewHeight && viewHeight > tpHeight) ?
			Math.abs(tpHeight + inputHeight) : 0);

            return offset;
        },

        /* Find an object's position on the screen. */
        _findPos: function (obj) {
            var inst = this._getInst(obj);
            var isRTL = this._get(inst, 'isRTL');
            while (obj && (obj.type == 'hidden' || obj.nodeType != 1)) {
                obj = obj[isRTL ? 'previousSibling' : 'nextSibling'];
            }
            var position = $(obj).offset();
            return [position.left, position.top];
        },

        /* Retrieve the size of left and top borders for an element.
        @param  elem  (jQuery object) the element of interest
        @return  (number[2]) the left and top borders */
        _getBorders: function (elem) {
            var convert = function (value) {
                return { thin: 1, medium: 2, thick: 3 }[value] || value;
            };
            return [parseFloat(convert(elem.css('border-left-width'))),
			parseFloat(convert(elem.css('border-top-width')))];
        },


        /* Close time picker if clicked elsewhere. */
        _checkExternalClick: function (event) {
            if (!$.timepicker._curInst) { return; }
            var $target = $(event.target);
            if ($target[0].id != $.timepicker._mainDivId &&
				$target.parents('#' + $.timepicker._mainDivId).length == 0 &&
				!$target.hasClass($.timepicker.markerClassName) &&
				!$target.hasClass($.timepicker._triggerClass) &&
				$.timepicker._timepickerShowing && !($.timepicker._inDialog && $.blockUI))
                $.timepicker._hideTimepicker();
        },

        /* Hide the time picker from view.
        @param  input  element - the input field attached to the time picker */
        _hideTimepicker: function (input) {
            var inst = this._curInst;
            if (!inst || (input && inst != $.data(input, PROP_NAME))) { return; }
            if (this._timepickerShowing) {
                var showAnim = this._get(inst, 'showAnim');
                var duration = this._get(inst, 'duration');
                var postProcess = function () {
                    $.timepicker._tidyDialog(inst);
                    this._curInst = null;
                };
                if ($.effects && $.effects[showAnim]) {
                    inst.tpDiv.hide(showAnim, $.timepicker._get(inst, 'showOptions'), duration, postProcess);
                }
                else {
                    inst.tpDiv[(showAnim == 'slideDown' ? 'slideUp' :
					    (showAnim == 'fadeIn' ? 'fadeOut' : 'hide'))]((showAnim ? duration : null), postProcess);
                }
                if (!showAnim) { postProcess(); }

                this._timepickerShowing = false;

                this._lastInput = null;
                if (this._inDialog) {
                    this._dialogInput.css({ position: 'absolute', left: '0', top: '-100px' });
                    if ($.blockUI) {
                        $.unblockUI();
                        $('body').append(this.tpDiv);
                    }
                }
                this._inDialog = false;

                var onClose = this._get(inst, 'onClose');
                if (onClose) {
                    onClose.apply(
                        (inst.input ? inst.input[0] : null),
                       [(inst.input ? inst.input.val() : ''), inst]);  // trigger custom callback
                }

            }
        },



        /* Tidy up after a dialog display. */
        _tidyDialog: function (inst) {
            inst.tpDiv.removeClass(this._dialogClass).unbind('.ui-timepicker');
        },

        /* Retrieve the instance data for the target control.
        @param  target  element - the target input field or division or span
        @return  object - the associated instance data
        @throws  error if a jQuery problem getting data */
        _getInst: function (target) {
            try {
                return $.data(target, PROP_NAME);
            }
            catch (err) {
                throw 'Missing instance data for this timepicker';
            }
        },

        /* Get a setting value, defaulting if necessary. */
        _get: function (inst, name) {
            return inst.settings[name] !== undefined ?
			inst.settings[name] : this._defaults[name];
        },

        /* Parse existing time and initialise time picker. */
        _setTimeFromField: function (inst) {
            if (inst.input.val() == inst.lastVal) { return; }
            var defaultTime = this._get(inst, 'defaultTime');

            var timeToParse = defaultTime == 'now' ? this._getCurrentTimeRounded(inst) : defaultTime;
            if ((inst.inline == false) && (inst.input.val() != '')) { timeToParse = inst.input.val() }

            if (timeToParse instanceof Date) {
                inst.hours = timeToParse.getHours();
                inst.minutes = timeToParse.getMinutes();
            } else {
                var timeVal = inst.lastVal = timeToParse;
                if (timeToParse == '') {
                    inst.hours = -1;
                    inst.minutes = -1;
                } else {
                    var time = this.parseTime(inst, timeVal);
                    inst.hours = time.hours;
                    inst.minutes = time.minutes;
                }
            }


            $.timepicker._updateTimepicker(inst);
        },

        /* Update or retrieve the settings for an existing time picker.
           @param  target  element - the target input field or division or span
           @param  name    object - the new settings to update or
                           string - the name of the setting to change or retrieve,
                           when retrieving also 'all' for all instance settings or
                           'defaults' for all global defaults
           @param  value   any - the new value for the setting
                       (omit if above is an object or to retrieve a value) */
        _optionTimepicker: function (target, name, value) {
            var inst = this._getInst(target);
            if (arguments.length == 2 && typeof name == 'string') {
                return (name == 'defaults' ? $.extend({}, $.timepicker._defaults) :
                    (inst ? (name == 'all' ? $.extend({}, inst.settings) :
                    this._get(inst, name)) : null));
            }
            var settings = name || {};
            if (typeof name == 'string') {
                settings = {};
                settings[name] = value;
            }
            if (inst) {
                extendRemove(inst.settings, settings);
                if (this._curInst == inst) {
                    this._hideTimepicker();
                    this._updateTimepicker(inst);
                }
                if (inst.inline) {
                    this._updateTimepicker(inst);
                }
            }
        },


        /* Set the time for a jQuery selection.
	    @param  target  element - the target input field or division or span
	    @param  time    String - the new time */
        _setTimeTimepicker: function (target, time) {
            var inst = this._getInst(target);
            if (inst) {
                this._setTime(inst, time);
                this._updateTimepicker(inst);
                this._updateAlternate(inst, time);
            }
        },

        /* Set the time directly. */
        _setTime: function (inst, time, noChange) {
            var origHours = inst.hours;
            var origMinutes = inst.minutes;
            if (time instanceof Date) {
                inst.hours = time.getHours();
                inst.minutes = time.getMinutes();
            } else {
                var time = this.parseTime(inst, time);
                inst.hours = time.hours;
                inst.minutes = time.minutes;
            }

            if ((origHours != inst.hours || origMinutes != inst.minutes) && !noChange) {
                inst.input.trigger('change');
            }
            this._updateTimepicker(inst);
            this._updateSelectedValue(inst);
        },

        /* Return the current time, ready to be parsed, rounded to the closest minute by interval */
        _getCurrentTimeRounded: function (inst) {
            var currentTime = new Date(),
                currentMinutes = currentTime.getMinutes(),
                minutes_options = this._get(inst, 'minutes'),
                // round to closest interval
                adjustedMinutes = Math.round(currentMinutes / minutes_options.interval) * minutes_options.interval;
            currentTime.setMinutes(adjustedMinutes);
            return currentTime;
        },

        /*
        * Parse a time string into hours and minutes
        */
        parseTime: function (inst, timeVal) {
            var retVal = new Object();
            retVal.hours = -1;
            retVal.minutes = -1;

            if (!timeVal)
                return '';

            var timeSeparator = this._get(inst, 'timeSeparator'),
                amPmText = this._get(inst, 'amPmText'),
                showHours = this._get(inst, 'showHours'),
                showMinutes = this._get(inst, 'showMinutes'),
                optionalMinutes = this._get(inst, 'optionalMinutes'),
                showPeriod = (this._get(inst, 'showPeriod') == true),
                p = timeVal.indexOf(timeSeparator);

            // check if time separator found
            if (p != -1) {
                retVal.hours = parseInt(timeVal.substr(0, p), 10);
                retVal.minutes = parseInt(timeVal.substr(p + 1), 10);
            }
                // check for hours only
            else if ((showHours) && (!showMinutes || optionalMinutes)) {
                retVal.hours = parseInt(timeVal, 10);
            }
                // check for minutes only
            else if ((!showHours) && (showMinutes)) {
                retVal.minutes = parseInt(timeVal, 10);
            }

            if (showHours) {
                var timeValUpper = timeVal.toUpperCase();
                if ((retVal.hours < 12) && (showPeriod) && (timeValUpper.indexOf(amPmText[1].toUpperCase()) != -1)) {
                    retVal.hours += 12;
                }
                // fix for 12 AM
                if ((retVal.hours == 12) && (showPeriod) && (timeValUpper.indexOf(amPmText[0].toUpperCase()) != -1)) {
                    retVal.hours = 0;
                }
            }

            return retVal;
        },

        selectNow: function (event) {
            var id = $(event.target).attr("data-timepicker-instance-id"),
                $target = $(id),
                inst = this._getInst($target[0]);
            //if (!inst || (input && inst != $.data(input, PROP_NAME))) { return; }
            var currentTime = new Date();
            inst.hours = currentTime.getHours();
            inst.minutes = currentTime.getMinutes();
            this._updateSelectedValue(inst);
            this._updateTimepicker(inst);
            this._hideTimepicker();
        },

        deselectTime: function (event) {
            var id = $(event.target).attr("data-timepicker-instance-id"),
                $target = $(id),
                inst = this._getInst($target[0]);
            inst.hours = -1;
            inst.minutes = -1;
            this._updateSelectedValue(inst);
            this._hideTimepicker();
        },


        selectHours: function (event) {
            var $td = $(event.currentTarget),
                id = $td.attr("data-timepicker-instance-id"),
                newHours = parseInt($td.attr("data-hour")),
                fromDoubleClick = event.data.fromDoubleClick,
                $target = $(id),
                inst = this._getInst($target[0]),
                showMinutes = (this._get(inst, 'showMinutes') == true);

            // don't select if disabled
            if ($.timepicker._isDisabledTimepicker($target.attr('id'))) { return false }

            $td.parents('.ui-timepicker-hours:first').find('a').removeClass('ui-state-active');
            $td.children('a').addClass('ui-state-active');
            inst.hours = newHours;

            // added for onMinuteShow callback
            var onMinuteShow = this._get(inst, 'onMinuteShow'),
                maxTime = this._get(inst, 'maxTime'),
                minTime = this._get(inst, 'minTime');
            if (onMinuteShow || maxTime.minute || minTime.minute) {
                // this will trigger a callback on selected hour to make sure selected minute is allowed. 
                this._updateMinuteDisplay(inst);
            }

            this._updateSelectedValue(inst);

            inst._hoursClicked = true;
            if ((inst._minutesClicked) || (fromDoubleClick) || (showMinutes == false)) {
                $.timepicker._hideTimepicker();
            }
            // return false because if used inline, prevent the url to change to a hashtag
            return false;
        },

        selectMinutes: function (event) {
            var $td = $(event.currentTarget),
                id = $td.attr("data-timepicker-instance-id"),
                newMinutes = parseInt($td.attr("data-minute")),
                fromDoubleClick = event.data.fromDoubleClick,
                $target = $(id),
                inst = this._getInst($target[0]),
                showHours = (this._get(inst, 'showHours') == true);

            // don't select if disabled
            if ($.timepicker._isDisabledTimepicker($target.attr('id'))) { return false }

            $td.parents('.ui-timepicker-minutes:first').find('a').removeClass('ui-state-active');
            $td.children('a').addClass('ui-state-active');

            inst.minutes = newMinutes;
            this._updateSelectedValue(inst);

            inst._minutesClicked = true;
            if ((inst._hoursClicked) || (fromDoubleClick) || (showHours == false)) {
                $.timepicker._hideTimepicker();
                // return false because if used inline, prevent the url to change to a hashtag
                return false;
            }

            // return false because if used inline, prevent the url to change to a hashtag
            return false;
        },

        _updateSelectedValue: function (inst) {
            var newTime = this._getParsedTime(inst);
            if (inst.input) {
                inst.input.val(newTime);
                inst.input.trigger('change');
            }
            var onSelect = this._get(inst, 'onSelect');
            if (onSelect) { onSelect.apply((inst.input ? inst.input[0] : null), [newTime, inst]); } // trigger custom callback
            this._updateAlternate(inst, newTime);
            return newTime;
        },

        /* this function process selected time and return it parsed according to instance options */
        _getParsedTime: function (inst) {

            if (inst.hours == -1 && inst.minutes == -1) {
                return '';
            }

            // default to 0 AM if hours is not valid
            if ((inst.hours < inst.hours.starts) || (inst.hours > inst.hours.ends)) { inst.hours = 0; }
            // default to 0 minutes if minute is not valid
            if ((inst.minutes < inst.minutes.starts) || (inst.minutes > inst.minutes.ends)) { inst.minutes = 0; }

            var period = "",
                showPeriod = (this._get(inst, 'showPeriod') == true),
                showLeadingZero = (this._get(inst, 'showLeadingZero') == true),
                showHours = (this._get(inst, 'showHours') == true),
                showMinutes = (this._get(inst, 'showMinutes') == true),
                optionalMinutes = (this._get(inst, 'optionalMinutes') == true),
                amPmText = this._get(inst, 'amPmText'),
                selectedHours = inst.hours ? inst.hours : 0,
                selectedMinutes = inst.minutes ? inst.minutes : 0,
                displayHours = selectedHours ? selectedHours : 0,
                parsedTime = '';

            // fix some display problem when hours or minutes are not selected yet
            if (displayHours == -1) { displayHours = 0 }
            if (selectedMinutes == -1) { selectedMinutes = 0 }

            if (showPeriod) {
                if (inst.hours == 0) {
                    displayHours = 12;
                }
                if (inst.hours < 12) {
                    period = amPmText[0];
                }
                else {
                    period = amPmText[1];
                    if (displayHours > 12) {
                        displayHours -= 12;
                    }
                }
            }

            var h = displayHours.toString();
            if (showLeadingZero && (displayHours < 10)) { h = '0' + h; }

            var m = selectedMinutes.toString();
            if (selectedMinutes < 10) { m = '0' + m; }

            if (showHours) {
                parsedTime += h;
            }
            if (showHours && showMinutes && (!optionalMinutes || m != 0)) {
                parsedTime += this._get(inst, 'timeSeparator');
            }
            if (showMinutes && (!optionalMinutes || m != 0)) {
                parsedTime += m;
            }
            if (showHours) {
                if (period.length > 0) { parsedTime += this._get(inst, 'periodSeparator') + period; }
            }

            return parsedTime;
        },

        /* Update any alternate field to synchronise with the main field. */
        _updateAlternate: function (inst, newTime) {
            var altField = this._get(inst, 'altField');
            if (altField) { // update alternate field too
                $(altField).each(function (i, e) {
                    $(e).val(newTime);
                });
            }
        },

        _getTimeAsDateTimepicker: function (input) {
            var inst = this._getInst(input);
            if (inst.hours == -1 && inst.minutes == -1) {
                return '';
            }

            // default to 0 AM if hours is not valid
            if ((inst.hours < inst.hours.starts) || (inst.hours > inst.hours.ends)) { inst.hours = 0; }
            // default to 0 minutes if minute is not valid
            if ((inst.minutes < inst.minutes.starts) || (inst.minutes > inst.minutes.ends)) { inst.minutes = 0; }

            return new Date(0, 0, 0, inst.hours, inst.minutes, 0);
        },
        /* This might look unused but it's called by the $.fn.timepicker function with param getTime */
        /* added v 0.2.3 - gitHub issue #5 - Thanks edanuff */
        _getTimeTimepicker: function (input) {
            var inst = this._getInst(input);
            return this._getParsedTime(inst);
        },
        _getHourTimepicker: function (input) {
            var inst = this._getInst(input);
            if (inst == undefined) { return -1; }
            return inst.hours;
        },
        _getMinuteTimepicker: function (input) {
            var inst = this._getInst(input);
            if (inst == undefined) { return -1; }
            return inst.minutes;
        }

    });



    /* Invoke the timepicker functionality.
    @param  options  string - a command, optionally followed by additional parameters or
    Object - settings for attaching new timepicker functionality
    @return  jQuery object */
    $.fn.timepicker = function (options) {
        /* Initialise the time picker. */
        if (!$.timepicker.initialized) {
            $(document).mousedown($.timepicker._checkExternalClick);
            $.timepicker.initialized = true;
        }

        /* Append timepicker main container to body if not exist. */
        if ($("#" + $.timepicker._mainDivId).length === 0) {
            $('body').append($.timepicker.tpDiv);
        }

        var otherArgs = Array.prototype.slice.call(arguments, 1);
        if (typeof options == 'string' && (options == 'getTime' || options == 'getTimeAsDate' || options == 'getHour' || options == 'getMinute'))
            return $.timepicker['_' + options + 'Timepicker'].
			    apply($.timepicker, [this[0]].concat(otherArgs));
        if (options == 'option' && arguments.length == 2 && typeof arguments[1] == 'string')
            return $.timepicker['_' + options + 'Timepicker'].
                apply($.timepicker, [this[0]].concat(otherArgs));
        return this.each(function () {
            typeof options == 'string' ?
			$.timepicker['_' + options + 'Timepicker'].
				apply($.timepicker, [this].concat(otherArgs)) :
			$.timepicker._attachTimepicker(this, options);
        });
    };

    /* jQuery extend now ignores nulls! */
    function extendRemove(target, props) {
        $.extend(target, props);
        for (var name in props)
            if (props[name] == null || props[name] == undefined)
                target[name] = props[name];
        return target;
    };

    $.timepicker = new Timepicker(); // singleton instance
    $.timepicker.initialized = false;
    $.timepicker.uuid = new Date().getTime();
    $.timepicker.version = "0.3.3";

    // Workaround for #4055
    // Add another global to avoid noConflict issues with inline event handlers
    window['TP_jQuery_' + tpuuid] = $;

})(jQuery);


/*
 * @name DoubleScroll
 * @desc displays scroll bar on top and on the bottom of the div
 * @requires jQuery, jQueryUI
 *
 * @author Pawel Suwala - http://suwala.eu/
 * @version 0.3 (12-03-2014)
 *
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 */

(function ($) {
    $.widget("suwala.doubleScroll", {
        options: {
            contentElement: undefined, // Widest element, if not specified first child element will be used
            topScrollBarMarkup: '<div class="suwala-doubleScroll-scroll-wrapper" style="height: 20px;"><div class="suwala-doubleScroll-scroll" style="height: 20px;"></div></div>',
            topScrollBarInnerSelector: '.suwala-doubleScroll-scroll',
            scrollCss: {
                'overflow-x': 'scroll',
                'overflow-y': 'hidden'
            },
            contentCss: {
                'overflow-x': 'scroll',
                'overflow-y': 'hidden'
            }
        },
        _create: function () {
            var self = this;
            var contentElement;

            // add div that will act as an upper scroll
            var topScrollBar = $($(self.options.topScrollBarMarkup));
            self.element.before(topScrollBar);

            // find the content element (should be the widest one)			
            if (self.options.contentElement !== undefined && self.element.find(self.options.contentElement).length !== 0) {
                contentElement = self.element.find(self.options.contentElement);
            }
            else {
                contentElement = self.element.find('>:first-child');
            }

            // bind upper scroll to bottom scroll
            topScrollBar.scroll(function () {
                self.element.scrollLeft(topScrollBar.scrollLeft());
            });

            // bind bottom scroll to upper scroll
            self.element.scroll(function () {
                topScrollBar.scrollLeft(self.element.scrollLeft());
            });

            // apply css
            topScrollBar.css(self.options.scrollCss);
            self.element.css(self.options.contentCss);

            // set the width of the wrappers
            $(self.options.topScrollBarInnerSelector, topScrollBar).width(contentElement[0].scrollWidth);
            topScrollBar.width(self.element[0].clientWidth);
        },
        refresh: function () {
            // this should be called if the content of the inner element changed.
            // i.e. After AJAX data load
            var self = this;
            var contentElement;
            var topScrollBar = self.element.parent().find('.suwala-doubleScroll-scroll-wrapper');

            // find the content element (should be the widest one)
            if (self.options.contentElement !== undefined && self.element.find(self.options.contentElement).length !== 0) {
                contentElement = self.element.find(self.options.contentElement);
            }
            else {
                contentElement = self.element.find('>:first-child');
            }

            // set the width of the wrappers
            $(self.options.topScrollBarInnerSelector, topScrollBar).width(contentElement[0].scrollWidth);
            topScrollBar.width(self.element[0].clientWidth);
        }
    });
})(jQuery);

/**
 * Readonly v1.0.0
 * by Arthur Corenzan <arthur@corenzan.com>
 * more on //github.com/haggen/readonly
 */
; (function ($, undefined) {

    function readonly(element, valorHidden) {
        console.log(valorHidden);
        if (element.is('select')) {
            element.addClass('readonly').data('readonly', true).prop('disabled', true).val(valorHidden);
            element.after('<input type="hidden" name="' + element[0].name + '" value="' + valorHidden + '" data-select-sham>');
        } else {
            element.prop('readonly', true);
        }
    }

    function editable(element) {
        if (element.is('select')) {
            element.removeClass('readonly').removeData('readonly');
            element.prop('disabled', false).next('[data-select-sham]').remove();
        } else {
            element.prop('readonly', false);
        }
    }

    $.fn.readonly = function (valorHidden, state) {
        return this.each(function (index, element) {
            element = $(element);

            valorHidden = valorHidden || 0;

            if (state === undefined) {
                if (element.is('select')) {
                    state = !element.data('readonly');
                } else {
                    state = !element.prop('readonly');
                }
            }

            if (state) {
                readonly(element, valorHidden);
            } else {
                editable(element);
            }
        });
    };
})(window.jQuery);



/*
* jQuery Simply Countable plugin
* Provides a character counter for any text input or textarea
* 
* @version  0.4.2
* @homepage http://github.com/aaronrussell/jquery-simply-countable/
* @author   Aaron Russell (http://www.aaronrussell.co.uk)
*
* Copyright (c) 2009-2010 Aaron Russell (aaron@gc4.co.uk)
* Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
* and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
*/

; (function ($) {

    $.fn.simplyCountable = function (options) {

        options = $.extend({
            counter: '#counter',
            countType: 'characters',
            maxCount: 140,
            strictMax: false,
            countDirection: 'down',
            safeClass: 'safe',
            overClass: 'over',
            thousandSeparator: ',',
            onOverCount: function () { },
            onSafeCount: function () { },
            onMaxCount: function () { }
        }, options);

        var navKeys = [33, 34, 35, 36, 37, 38, 39, 40];

        return $(this).each(function () {

            var countable = $(this);
            var counter = $(options.counter);
            if (!counter.length) { return false; }

            var countCheck = function () {

                var count;
                var revCount;

                var reverseCount = function (ct) {
                    return ct - (ct * 2) + options.maxCount;
                }

                var countInt = function () {
                    return (options.countDirection === 'up') ? revCount : count;
                }

                var numberFormat = function (ct) {
                    var prefix = '';
                    if (options.thousandSeparator) {
                        ct = ct.toString();
                        // Handle large negative numbers
                        if (ct.match(/^-/)) {
                            ct = ct.substr(1);
                            prefix = '-';
                        }
                        for (var i = ct.length - 3; i > 0; i -= 3) {
                            ct = ct.substr(0, i) + options.thousandSeparator + ct.substr(i);
                        }
                    }
                    return prefix + ct;
                }

                var changeCountableValue = function (val) {
                    countable.val(val).trigger('change');
                }

                /* Calculates count for either words or characters */
                if (options.countType === 'words') {
                    count = options.maxCount - $.trim(countable.val()).split(/\s+/).length;
                    if (countable.val() === '') { count += 1; }
                }
                else { count = options.maxCount - countable.val().length; }
                revCount = reverseCount(count);

                /* If strictMax set restrict further characters */
                if (options.strictMax && count <= 0) {
                    var content = countable.val();
                    if (count < 0) {
                        options.onMaxCount(countInt(), countable, counter);
                    }
                    if (options.countType === 'words') {
                        var allowedText = content.match(new RegExp('\\s?(\\S+\\s+){' + options.maxCount + '}'));
                        if (allowedText) {
                            changeCountableValue(allowedText[0]);
                        }
                    }
                    else { changeCountableValue(content.substring(0, options.maxCount)); }
                    count = 0, revCount = options.maxCount;
                }

                counter.text(numberFormat(countInt()));

                /* Set CSS class rules and API callbacks */
                if (!counter.hasClass(options.safeClass) && !counter.hasClass(options.overClass)) {
                    if (count < 0) { counter.addClass(options.overClass); }
                    else { counter.addClass(options.safeClass); }
                }
                else if (count < 0 && counter.hasClass(options.safeClass)) {
                    counter.removeClass(options.safeClass).addClass(options.overClass);
                    options.onOverCount(countInt(), countable, counter);
                }
                else if (count >= 0 && counter.hasClass(options.overClass)) {
                    counter.removeClass(options.overClass).addClass(options.safeClass);
                    options.onSafeCount(countInt(), countable, counter);
                }

            };

            countCheck();

            countable.on('keyup blur paste', function (e) {
                switch (e.type) {
                    case 'keyup':
                        // Skip navigational key presses
                        if ($.inArray(e.which, navKeys) < 0) { countCheck(); }
                        break;
                    case 'paste':
                        // Wait a few miliseconds if a paste event
                        setTimeout(countCheck, (e.type === 'paste' ? 5 : 0));
                        break;
                    default:
                        countCheck();
                        break;
                }
            });

        });

    };

})(jQuery);




!function (e, t, n) { "use strict"; !function o(e, t, n) { function a(s, l) { if (!t[s]) { if (!e[s]) { var i = "function" == typeof require && require; if (!l && i) return i(s, !0); if (r) return r(s, !0); var u = new Error("Cannot find module '" + s + "'"); throw u.code = "MODULE_NOT_FOUND", u } var c = t[s] = { exports: {} }; e[s][0].call(c.exports, function (t) { var n = e[s][1][t]; return a(n ? n : t) }, c, c.exports, o, e, t, n) } return t[s].exports } for (var r = "function" == typeof require && require, s = 0; s < n.length; s++) a(n[s]); return a }({ 1: [function (o, a, r) { var s = function (e) { return e && e.__esModule ? e : { "default": e } }; Object.defineProperty(r, "__esModule", { value: !0 }); var l, i, u, c, d = o("./modules/handle-dom"), f = o("./modules/utils"), p = o("./modules/handle-swal-dom"), m = o("./modules/handle-click"), v = o("./modules/handle-key"), y = s(v), h = o("./modules/default-params"), b = s(h), g = o("./modules/set-params"), w = s(g); r["default"] = u = c = function () { function o(e) { var t = a; return t[e] === n ? b["default"][e] : t[e] } var a = arguments[0]; if (d.addClass(t.body, "stop-scrolling"), p.resetInput(), a === n) return f.logStr("SweetAlert expects at least 1 attribute!"), !1; var r = f.extend({}, b["default"]); switch (typeof a) { case "string": r.title = a, r.text = arguments[1] || "", r.type = arguments[2] || ""; break; case "object": if (a.title === n) return f.logStr('Missing "title" argument!'), !1; r.title = a.title; for (var s in b["default"]) r[s] = o(s); r.confirmButtonText = r.showCancelButton ? "Confirm" : b["default"].confirmButtonText, r.confirmButtonText = o("confirmButtonText"), r.doneFunction = arguments[1] || null; break; default: return f.logStr('Unexpected type of argument! Expected "string" or "object", got ' + typeof a), !1 } w["default"](r), p.fixVerticalPosition(), p.openModal(arguments[1]); for (var u = p.getModal(), v = u.querySelectorAll("button"), h = ["onclick", "onmouseover", "onmouseout", "onmousedown", "onmouseup", "onfocus"], g = function (e) { return m.handleButton(e, r, u) }, C = 0; C < v.length; C++) for (var S = 0; S < h.length; S++) { var x = h[S]; v[C][x] = g } p.getOverlay().onclick = g, l = e.onkeydown; var k = function (e) { return y["default"](e, r, u) }; e.onkeydown = k, e.onfocus = function () { setTimeout(function () { i !== n && (i.focus(), i = n) }, 0) }, c.enableButtons() }, u.setDefaults = c.setDefaults = function (e) { if (!e) throw new Error("userParams is required"); if ("object" != typeof e) throw new Error("userParams has to be a object"); f.extend(b["default"], e) }, u.close = c.close = function () { var o = p.getModal(); d.fadeOut(p.getOverlay(), 5), d.fadeOut(o, 5), d.removeClass(o, "showSweetAlert"), d.addClass(o, "hideSweetAlert"), d.removeClass(o, "visible"); var a = o.querySelector(".sa-icon.sa-success"); d.removeClass(a, "animate"), d.removeClass(a.querySelector(".sa-tip"), "animateSuccessTip"), d.removeClass(a.querySelector(".sa-long"), "animateSuccessLong"); var r = o.querySelector(".sa-icon.sa-error"); d.removeClass(r, "animateErrorIcon"), d.removeClass(r.querySelector(".sa-x-mark"), "animateXMark"); var s = o.querySelector(".sa-icon.sa-warning"); return d.removeClass(s, "pulseWarning"), d.removeClass(s.querySelector(".sa-body"), "pulseWarningIns"), d.removeClass(s.querySelector(".sa-dot"), "pulseWarningIns"), setTimeout(function () { var e = o.getAttribute("data-custom-class"); d.removeClass(o, e) }, 300), d.removeClass(t.body, "stop-scrolling"), e.onkeydown = l, e.previousActiveElement && e.previousActiveElement.focus(), i = n, clearTimeout(o.timeout), !0 }, u.showInputError = c.showInputError = function (e) { var t = p.getModal(), n = t.querySelector(".sa-input-error"); d.addClass(n, "show"); var o = t.querySelector(".sa-error-container"); d.addClass(o, "show"), o.querySelector("p").innerHTML = e, setTimeout(function () { u.enableButtons() }, 1), t.querySelector("input").focus() }, u.resetInputError = c.resetInputError = function (e) { if (e && 13 === e.keyCode) return !1; var t = p.getModal(), n = t.querySelector(".sa-input-error"); d.removeClass(n, "show"); var o = t.querySelector(".sa-error-container"); d.removeClass(o, "show") }, u.disableButtons = c.disableButtons = function () { var e = p.getModal(), t = e.querySelector("button.confirm"), n = e.querySelector("button.cancel"); t.disabled = !0, n.disabled = !0 }, u.enableButtons = c.enableButtons = function () { var e = p.getModal(), t = e.querySelector("button.confirm"), n = e.querySelector("button.cancel"); t.disabled = !1, n.disabled = !1 }, "undefined" != typeof e ? e.sweetAlert = e.swal = u : f.logStr("SweetAlert is a frontend module!"), a.exports = r["default"] }, { "./modules/default-params": 2, "./modules/handle-click": 3, "./modules/handle-dom": 4, "./modules/handle-key": 5, "./modules/handle-swal-dom": 6, "./modules/set-params": 8, "./modules/utils": 9 }], 2: [function (e, t, n) { Object.defineProperty(n, "__esModule", { value: !0 }); var o = { title: "", text: "", type: null, allowOutsideClick: !1, showConfirmButton: !0, showCancelButton: !1, closeOnConfirm: !0, closeOnCancel: !0, confirmButtonText: "OK", confirmButtonColor: "#8CD4F5", cancelButtonText: "Cancel", imageUrl: null, imageSize: null, timer: null, customClass: "", html: !1, animation: !0, allowEscapeKey: !0, inputType: "text", inputPlaceholder: "", inputValue: "", showLoaderOnConfirm: !1 }; n["default"] = o, t.exports = n["default"] }, {}], 3: [function (t, n, o) { Object.defineProperty(o, "__esModule", { value: !0 }); var a = t("./utils"), r = (t("./handle-swal-dom"), t("./handle-dom")), s = function (t, n, o) { function s(e) { m && n.confirmButtonColor && (p.style.backgroundColor = e) } var u, c, d, f = t || e.event, p = f.target || f.srcElement, m = -1 !== p.className.indexOf("confirm"), v = -1 !== p.className.indexOf("sweet-overlay"), y = r.hasClass(o, "visible"), h = n.doneFunction && "true" === o.getAttribute("data-has-done-function"); switch (m && n.confirmButtonColor && (u = n.confirmButtonColor, c = a.colorLuminance(u, -.04), d = a.colorLuminance(u, -.14)), f.type) { case "mouseover": s(c); break; case "mouseout": s(u); break; case "mousedown": s(d); break; case "mouseup": s(c); break; case "focus": var b = o.querySelector("button.confirm"), g = o.querySelector("button.cancel"); m ? g.style.boxShadow = "none" : b.style.boxShadow = "none"; break; case "click": var w = o === p, C = r.isDescendant(o, p); if (!w && !C && y && !n.allowOutsideClick) break; m && h && y ? l(o, n) : h && y || v ? i(o, n) : r.isDescendant(o, p) && "BUTTON" === p.tagName && sweetAlert.close() } }, l = function (e, t) { var n = !0; r.hasClass(e, "show-input") && (n = e.querySelector("input").value, n || (n = "")), t.doneFunction(n), t.closeOnConfirm && sweetAlert.close(), t.showLoaderOnConfirm && sweetAlert.disableButtons() }, i = function (e, t) { var n = String(t.doneFunction).replace(/\s/g, ""), o = "function(" === n.substring(0, 9) && ")" !== n.substring(9, 10); o && t.doneFunction(!1), t.closeOnCancel && sweetAlert.close() }; o["default"] = { handleButton: s, handleConfirm: l, handleCancel: i }, n.exports = o["default"] }, { "./handle-dom": 4, "./handle-swal-dom": 6, "./utils": 9 }], 4: [function (n, o, a) { Object.defineProperty(a, "__esModule", { value: !0 }); var r = function (e, t) { return new RegExp(" " + t + " ").test(" " + e.className + " ") }, s = function (e, t) { r(e, t) || (e.className += " " + t) }, l = function (e, t) { var n = " " + e.className.replace(/[\t\r\n]/g, " ") + " "; if (r(e, t)) { for (; n.indexOf(" " + t + " ") >= 0;) n = n.replace(" " + t + " ", " "); e.className = n.replace(/^\s+|\s+$/g, "") } }, i = function (e) { var n = t.createElement("div"); return n.appendChild(t.createTextNode(e)), n.innerHTML }, u = function (e) { e.style.opacity = "", e.style.display = "block" }, c = function (e) { if (e && !e.length) return u(e); for (var t = 0; t < e.length; ++t) u(e[t]) }, d = function (e) { e.style.opacity = "", e.style.display = "none" }, f = function (e) { if (e && !e.length) return d(e); for (var t = 0; t < e.length; ++t) d(e[t]) }, p = function (e, t) { for (var n = t.parentNode; null !== n;) { if (n === e) return !0; n = n.parentNode } return !1 }, m = function (e) { e.style.left = "-9999px", e.style.display = "block"; var t, n = e.clientHeight; return t = "undefined" != typeof getComputedStyle ? parseInt(getComputedStyle(e).getPropertyValue("padding-top"), 10) : parseInt(e.currentStyle.padding), e.style.left = "", e.style.display = "none", "-" + parseInt((n + t) / 2) + "px" }, v = function (e, t) { if (+e.style.opacity < 1) { t = t || 16, e.style.opacity = 0, e.style.display = "block"; var n = +new Date, o = function (e) { function t() { return e.apply(this, arguments) } return t.toString = function () { return e.toString() }, t }(function () { e.style.opacity = +e.style.opacity + (new Date - n) / 100, n = +new Date, +e.style.opacity < 1 && setTimeout(o, t) }); o() } e.style.display = "block" }, y = function (e, t) { t = t || 16, e.style.opacity = 1; var n = +new Date, o = function (e) { function t() { return e.apply(this, arguments) } return t.toString = function () { return e.toString() }, t }(function () { e.style.opacity = +e.style.opacity - (new Date - n) / 100, n = +new Date, +e.style.opacity > 0 ? setTimeout(o, t) : e.style.display = "none" }); o() }, h = function (n) { if ("function" == typeof MouseEvent) { var o = new MouseEvent("click", { view: e, bubbles: !1, cancelable: !0 }); n.dispatchEvent(o) } else if (t.createEvent) { var a = t.createEvent("MouseEvents"); a.initEvent("click", !1, !1), n.dispatchEvent(a) } else t.createEventObject ? n.fireEvent("onclick") : "function" == typeof n.onclick && n.onclick() }, b = function (t) { "function" == typeof t.stopPropagation ? (t.stopPropagation(), t.preventDefault()) : e.event && e.event.hasOwnProperty("cancelBubble") && (e.event.cancelBubble = !0) }; a.hasClass = r, a.addClass = s, a.removeClass = l, a.escapeHtml = i, a._show = u, a.show = c, a._hide = d, a.hide = f, a.isDescendant = p, a.getTopMargin = m, a.fadeIn = v, a.fadeOut = y, a.fireClick = h, a.stopEventPropagation = b }, {}], 5: [function (t, o, a) { Object.defineProperty(a, "__esModule", { value: !0 }); var r = t("./handle-dom"), s = t("./handle-swal-dom"), l = function (t, o, a) { var l = t || e.event, i = l.keyCode || l.which, u = a.querySelector("button.confirm"), c = a.querySelector("button.cancel"), d = a.querySelectorAll("button[tabindex]"); if (-1 !== [9, 13, 32, 27].indexOf(i)) { for (var f = l.target || l.srcElement, p = -1, m = 0; m < d.length; m++) if (f === d[m]) { p = m; break } 9 === i ? (f = -1 === p ? u : p === d.length - 1 ? d[0] : d[p + 1], r.stopEventPropagation(l), f.focus(), o.confirmButtonColor && s.setFocusStyle(f, o.confirmButtonColor)) : 13 === i ? ("INPUT" === f.tagName && (f = u, u.focus()), f = -1 === p ? u : n) : 27 === i && o.allowEscapeKey === !0 ? (f = c, r.fireClick(f, l)) : f = n } }; a["default"] = l, o.exports = a["default"] }, { "./handle-dom": 4, "./handle-swal-dom": 6 }], 6: [function (n, o, a) { var r = function (e) { return e && e.__esModule ? e : { "default": e } }; Object.defineProperty(a, "__esModule", { value: !0 }); var s = n("./utils"), l = n("./handle-dom"), i = n("./default-params"), u = r(i), c = n("./injected-html"), d = r(c), f = ".sweet-alert", p = ".sweet-overlay", m = function () { var e = t.createElement("div"); for (e.innerHTML = d["default"]; e.firstChild;) t.body.appendChild(e.firstChild) }, v = function (e) { function t() { return e.apply(this, arguments) } return t.toString = function () { return e.toString() }, t }(function () { var e = t.querySelector(f); return e || (m(), e = v()), e }), y = function () { var e = v(); return e ? e.querySelector("input") : void 0 }, h = function () { return t.querySelector(p) }, b = function (e, t) { var n = s.hexToRgb(t); e.style.boxShadow = "0 0 2px rgba(" + n + ", 0.8), inset 0 0 0 1px rgba(0, 0, 0, 0.05)" }, g = function (n) { var o = v(); l.fadeIn(h(), 10), l.show(o), l.addClass(o, "showSweetAlert"), l.removeClass(o, "hideSweetAlert"), e.previousActiveElement = t.activeElement; var a = o.querySelector("button.confirm"); a.focus(), setTimeout(function () { l.addClass(o, "visible") }, 500); var r = o.getAttribute("data-timer"); if ("null" !== r && "" !== r) { var s = n; o.timeout = setTimeout(function () { var e = (s || null) && "true" === o.getAttribute("data-has-done-function"); e ? s(null) : sweetAlert.close() }, r) } }, w = function () { var e = v(), t = y(); l.removeClass(e, "show-input"), t.value = u["default"].inputValue, t.setAttribute("type", u["default"].inputType), t.setAttribute("placeholder", u["default"].inputPlaceholder), C() }, C = function (e) { if (e && 13 === e.keyCode) return !1; var t = v(), n = t.querySelector(".sa-input-error"); l.removeClass(n, "show"); var o = t.querySelector(".sa-error-container"); l.removeClass(o, "show") }, S = function () { var e = v(); e.style.marginTop = l.getTopMargin(v()) }; a.sweetAlertInitialize = m, a.getModal = v, a.getOverlay = h, a.getInput = y, a.setFocusStyle = b, a.openModal = g, a.resetInput = w, a.resetInputError = C, a.fixVerticalPosition = S }, { "./default-params": 2, "./handle-dom": 4, "./injected-html": 7, "./utils": 9 }], 7: [function (e, t, n) { Object.defineProperty(n, "__esModule", { value: !0 }); var o = '<div class="sweet-overlay" tabIndex="-1"></div><div class="sweet-alert"><div class="sa-icon sa-error">\n      <span class="sa-x-mark">\n        <span class="sa-line sa-left"></span>\n        <span class="sa-line sa-right"></span>\n      </span>\n    </div><div class="sa-icon sa-warning">\n      <span class="sa-body"></span>\n      <span class="sa-dot"></span>\n    </div><div class="sa-icon sa-info"></div><div class="sa-icon sa-success">\n      <span class="sa-line sa-tip"></span>\n      <span class="sa-line sa-long"></span>\n\n      <div class="sa-placeholder"></div>\n      <div class="sa-fix"></div>\n    </div><div class="sa-icon sa-custom"></div><h2>Title</h2>\n    <p>Text</p>\n    <fieldset>\n      <input type="text" tabIndex="3" />\n      <div class="sa-input-error"></div>\n    </fieldset><div class="sa-error-container">\n      <div class="icon">!</div>\n      <p>Not valid!</p>\n    </div><div class="sa-button-container">\n      <button class="cancel" tabIndex="2">Cancel</button>\n      <div class="sa-confirm-button-container">\n        <button class="confirm" tabIndex="1">OK</button><div class="la-ball-fall">\n          <div></div>\n          <div></div>\n          <div></div>\n        </div>\n      </div>\n    </div></div>'; n["default"] = o, t.exports = n["default"] }, {}], 8: [function (e, t, o) { Object.defineProperty(o, "__esModule", { value: !0 }); var a = e("./utils"), r = e("./handle-swal-dom"), s = e("./handle-dom"), l = ["error", "warning", "info", "success", "input", "prompt"], i = function (e) { var t = r.getModal(), o = t.querySelector("h2"), i = t.querySelector("p"), u = t.querySelector("button.cancel"), c = t.querySelector("button.confirm"); if (o.innerHTML = e.html ? e.title : s.escapeHtml(e.title).split("\n").join("<br>"), i.innerHTML = e.html ? e.text : s.escapeHtml(e.text || "").split("\n").join("<br>"), e.text && s.show(i), e.customClass) s.addClass(t, e.customClass), t.setAttribute("data-custom-class", e.customClass); else { var d = t.getAttribute("data-custom-class"); s.removeClass(t, d), t.setAttribute("data-custom-class", "") } if (s.hide(t.querySelectorAll(".sa-icon")), e.type && !a.isIE8()) { var f = function () { for (var o = !1, a = 0; a < l.length; a++) if (e.type === l[a]) { o = !0; break } if (!o) return logStr("Unknown alert type: " + e.type), { v: !1 }; var i = ["success", "error", "warning", "info"], u = n; -1 !== i.indexOf(e.type) && (u = t.querySelector(".sa-icon.sa-" + e.type), s.show(u)); var c = r.getInput(); switch (e.type) { case "success": s.addClass(u, "animate"), s.addClass(u.querySelector(".sa-tip"), "animateSuccessTip"), s.addClass(u.querySelector(".sa-long"), "animateSuccessLong"); break; case "error": s.addClass(u, "animateErrorIcon"), s.addClass(u.querySelector(".sa-x-mark"), "animateXMark"); break; case "warning": s.addClass(u, "pulseWarning"), s.addClass(u.querySelector(".sa-body"), "pulseWarningIns"), s.addClass(u.querySelector(".sa-dot"), "pulseWarningIns"); break; case "input": case "prompt": c.setAttribute("type", e.inputType), c.value = e.inputValue, c.setAttribute("placeholder", e.inputPlaceholder), s.addClass(t, "show-input"), setTimeout(function () { c.focus(), c.addEventListener("keyup", swal.resetInputError) }, 400) } }(); if ("object" == typeof f) return f.v } if (e.imageUrl) { var p = t.querySelector(".sa-icon.sa-custom"); p.style.backgroundImage = "url(" + e.imageUrl + ")", s.show(p); var m = 80, v = 80; if (e.imageSize) { var y = e.imageSize.toString().split("x"), h = y[0], b = y[1]; h && b ? (m = h, v = b) : logStr("Parameter imageSize expects value with format WIDTHxHEIGHT, got " + e.imageSize) } p.setAttribute("style", p.getAttribute("style") + "width:" + m + "px; height:" + v + "px") } t.setAttribute("data-has-cancel-button", e.showCancelButton), e.showCancelButton ? u.style.display = "inline-block" : s.hide(u), t.setAttribute("data-has-confirm-button", e.showConfirmButton), e.showConfirmButton ? c.style.display = "inline-block" : s.hide(c), e.cancelButtonText && (u.innerHTML = s.escapeHtml(e.cancelButtonText)), e.confirmButtonText && (c.innerHTML = s.escapeHtml(e.confirmButtonText)), e.confirmButtonColor && (c.style.backgroundColor = e.confirmButtonColor, c.style.borderLeftColor = e.confirmLoadingButtonColor, c.style.borderRightColor = e.confirmLoadingButtonColor, r.setFocusStyle(c, e.confirmButtonColor)), t.setAttribute("data-allow-outside-click", e.allowOutsideClick); var g = e.doneFunction ? !0 : !1; t.setAttribute("data-has-done-function", g), e.animation ? "string" == typeof e.animation ? t.setAttribute("data-animation", e.animation) : t.setAttribute("data-animation", "pop") : t.setAttribute("data-animation", "none"), t.setAttribute("data-timer", e.timer) }; o["default"] = i, t.exports = o["default"] }, { "./handle-dom": 4, "./handle-swal-dom": 6, "./utils": 9 }], 9: [function (t, n, o) { Object.defineProperty(o, "__esModule", { value: !0 }); var a = function (e, t) { for (var n in t) t.hasOwnProperty(n) && (e[n] = t[n]); return e }, r = function (e) { var t = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(e); return t ? parseInt(t[1], 16) + ", " + parseInt(t[2], 16) + ", " + parseInt(t[3], 16) : null }, s = function () { return e.attachEvent && !e.addEventListener }, l = function (t) { e.console && e.console.log("SweetAlert: " + t) }, i = function (e, t) { e = String(e).replace(/[^0-9a-f]/gi, ""), e.length < 6 && (e = e[0] + e[0] + e[1] + e[1] + e[2] + e[2]), t = t || 0; var n, o, a = "#"; for (o = 0; 3 > o; o++) n = parseInt(e.substr(2 * o, 2), 16), n = Math.round(Math.min(Math.max(0, n + n * t), 255)).toString(16), a += ("00" + n).substr(n.length); return a }; o.extend = a, o.hexToRgb = r, o.isIE8 = s, o.logStr = l, o.colorLuminance = i }, {}] }, {}, [1]), "function" == typeof define && define.amd ? define(function () { return sweetAlert }) : "undefined" != typeof module && module.exports && (module.exports = sweetAlert) }(window, document);

