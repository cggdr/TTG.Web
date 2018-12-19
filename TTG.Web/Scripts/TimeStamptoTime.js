//时间戳转换成日期
function getLocalTime(nS, type) {
    /*
     * nS:为传进来的时间戳
     * type:时间显示模式.若传入12则为12小时制,不传入则为24小时制
     */
    //年月日时分秒
    var Y, M, D, W, H, I, S;
    //月日时分秒为单位时前面补零
    function fillZero(v) {
        if (v < 10) { v = '0' + v; }
        return v;
    }
    var d = new Date(parseInt(nS / 1000) * 1000);
    var Week = ['星期天', '星期一', '星期二', '星期三', '星期四', '星期五', '星期六'];
    Y = d.getFullYear();
    M = fillZero(d.getMonth() + 1);
    D = fillZero(d.getDate());
    W = Week[d.getDay()];
    H = fillZero(d.getHours());
    I = fillZero(d.getMinutes());
    S = fillZero(d.getSeconds());
    //12小时制显示模式
    if (type && type == 12) {
        //若要显示更多时间类型诸如中午凌晨可在下面添加判断
        if (H <= 12) {
            H = '上午&nbsp;' + H;
        } else if (H > 12 && H < 24) {
            H -= 12;
            H = '下午&nbsp;' + fillZero(H);
        } else if (H == 24) {
            H = '下午&nbsp;00';
        }
    }
    var localTime = Y + '年' + M + '月' + D + '日 ';//+ H + ':' + I + ':' + S
    //    console.log(localTime);
    return localTime;
    //每秒更新时间
    // setTimeout(arguments.callee,1000);
}

