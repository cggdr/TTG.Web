
       $("#getCode").click(function () {
          
           //var p = $('#PhoneNumber').val();
           if (isPoneAvailable($('#PhoneNumber')) == false) {
               $('#success').text("请输入合适的手机号");
           }
           else {
               //@* $.post("url",{key:"value",..},function(data){},datatype) *@
               $.post("@Url.Action('GetCheckNum', 'User')", { phoneNum: $('#PhoneNumber').val() }, function (data) {
                   if (data.msg) {
                       $('#success').text("已发送验证码");
                       checkGetCodeBtn();
                       
                   } else {
                       var $getCodeBtn = $('#getCode');
                       clearInterval(t);
                       $getCodeBtn.prop('disabled', false);
                       $getCodeBtn.val("点击获取验证码");
                       count = 60;
                       $('#success').text("");
                       alert(data.content);
                       alert(2);
                   }
               }, "json")
           }
       });
var count = 60; //计时开始
var t; //时间间隔种子
var isPass = false;//验证码是否输入正确
function checkGetCodeBtn() {
    //关于按钮
    var $getCodeBtn = $('#getCode');

    t = setInterval(function () {
               
        $getCodeBtn.val(count + "秒后获取");
        $getCodeBtn.prop('disabled', true);
        count--;
               
        if (count == 0) {
            clearInterval(t);
            $getCodeBtn.prop('disabled', false);
            $getCodeBtn.val("获取验证码");
            count = 60;
            $('#success').text("");
        }
    }, 1000);
}
//判断是否为手机号
function isPoneAvailable($poneInput) {
    var myreg = /^[1][3,4,5,6,7,8,9][0-9]{9}$/;
    if (!myreg.test($poneInput.val())) {
        return false;
    } else {
        return true;
    }
}
