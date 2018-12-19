using System.Linq;
using TTG.Helper;
using LinqKit;
using System;
using System.Data.Entity;

namespace TTG.Core
{
    /// <summary>
    /// 用户管理类
    /// </summary>
    public class UserManager : BaseManager<User>
    {
        private UserIdentyManager _identy = new UserIdentyManager();
        /// <summary>
        /// 添加【返回值Response.Code:0-失败，1-成功，2-账号已存在，3-Email已存在】
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        public override Response Add(User user)
        {
            Response _resp = new Response();
            //账号是否存在
            if (!string.IsNullOrEmpty(user.PhoneNumber) && HasUsername(user.PhoneNumber))
            {
                _resp.Code = 2;
                _resp.Message = "用户名已存在";
            }
            //Email是否存在
            if (!string.IsNullOrEmpty(user.Email) && HasUsername(user.Email))
            {
                _resp.Code = 3;
                _resp.Message = "Email已存在";
            }
            if (_resp.Code == 0) _resp = base.Add(user);
            return _resp;
        }
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pagingUser">分页数据</param>
        /// <param name="roleID">角色ID</param>
        /// <param name="username">用户名</param>
        /// <param name="name">名称</param>
        /// <param name="sex">性别</param>
        /// <param name="email">Email</param>
        /// <param name="order">排序【null（默认）-ID降序，0-ID升序，1-ID降序，2-注册时间降序，3-注册时间升序，4-最后登录时间升序，5-最后登录时间降序】</param>
        /// <returns></returns>
        public Paging<User> FindPageList(Paging<User> pagingUser, int? roleID, string username, string name, int? sex, string email, int? order)
        {
          
            //查询表达式
            var _where = PredicateBuilder.True<User>();
            if (roleID != null && roleID > 0) _where = _where.And(u => u.RoleID == roleID);
            if (!string.IsNullOrEmpty(username)) _where = _where.And(u => u.PhoneNumber.Contains(username));
            if (!string.IsNullOrEmpty(name)) _where = _where.And(u => u.Name.Contains(name));
            if (sex != null && sex >= 0 && sex <= 2) _where = _where.And(u => u.Sex == sex);
            if (!string.IsNullOrEmpty(email)) _where = _where.And(u => u.Email.Contains(email));
            //排序
            OrderParam _orderParam;
            switch (order)
            {
                case 0://ID升序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.ASC };
                    break;
                case 1://ID降序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.DESC };
                    break;
                case 2://注册时间升序
                    _orderParam = new OrderParam() { PropertyName = "RegTime", Method = OrderMethod.ASC };
                    break;
                case 3://注册时间降序
                    _orderParam = new OrderParam() { PropertyName = "RegTime", Method = OrderMethod.DESC };
                    break;
                case 4://最后登录时间升序
                    _orderParam = new OrderParam() { PropertyName = "LastLoginTime", Method = OrderMethod.ASC };
                    break;
                case 5://最后登录时间降序
                    _orderParam = new OrderParam() { PropertyName = "LastLoginTime", Method = OrderMethod.DESC };
                    break;
                default://ID降序
                    _orderParam = new OrderParam() { PropertyName = "UserID", Method = OrderMethod.DESC };
                    break;
            }
            pagingUser.Items = Repository.FindPageList(pagingUser.PageSize, pagingUser.PageIndex, out pagingUser.TotalNumber, _where.Expand(), _orderParam).ToList();
            return pagingUser;
        }

        /// <summary>
        /// Email是否存在
        /// </summary>
        /// <param name="email">Email[不区分大小写]</param>
        /// <returns></returns>
        public bool HasEmail(string email)
        {
            return base.Repository.IsContains(u => u.Email.ToUpper() == email.ToUpper());
        }

        /// <summary>
        /// 用户名是否存在
        /// </summary>
        /// <param name="accounts">用户名[不区分大小写]</param>
        /// <returns></returns>
        public bool HasUsername(string phoneNum)
        {
            return base.Repository.IsContains(u => u.PhoneNumber == phoneNum);
        }
       
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userID">主键</param>
        /// <param name="password">新密码【密文】</param>
        /// <returns></returns>
        public Response ChangePassword(int UserID, string password)
        {
            Response _resp = new Response();
            var _User = Find(UserID);
            if (_User == null)
            {
                _resp.Code = 0;
                _resp.Message = "该用户不存在";
            }
            else
            {
                _User.Password = password;
                _resp = Update(_User);
            }
            return _resp;
        }
        /// <summary>
        /// 修改交易密码
        /// </summary>
        /// <param name="UserrID">主键</param>
        /// <param name="password">新密码【密文】</param>
        /// <returns></returns>
        public Response ChangeTransactionPassword(int UserID, string password)
        {
            Response _resp = new Response();
            
            var _User = Find(UserID);
            int id = _User.FKIdentyID;
            if (_User == null)
            {
                _resp.Code = 0;
                _resp.Message = "该用户不存在";
            }
            else
            {
                UserIdenty identy = _identy.Find(id);
                identy.Password = password;
                _resp = _identy.Update(identy);
            }
            return _resp;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="administratorID">主键</param>
        /// <returns></returns>




        /// <summary>
        /// 查找
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public User Find(string username)
        {
            return base.Repository.Find(a => a.PhoneNumber == username);
        }

        /// <summary>
        /// 帐号是否存在
        /// </summary>
        /// <param name="accounts">帐号[不区分大小写]</param>
        /// <returns></returns>
        public bool HasAccounts(string username)
        {
            return base.Repository.IsContains(a => a.PhoneNumber.ToUpper() == username.ToUpper());
        }

        /// <summary>
        /// 更新登录信息
        /// </summary>
        /// <param name="administratorID">主键</param>
        /// <param name="ip">IP地址</param>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public Response UpadateLoginInfo(int userID, string ip, DateTime time)
        {
            Response _resp = new Response();
            var _user = Find(userID);
            if (_user == null)
            {
                _resp.Code = 0;
                _resp.Message = "用户不存在";
            }
            else
            {
                _user.LastLoginIP = ip;
                _user.LastLoginTime = time;
                _resp = Update(_user);
            }
            return _resp;
        }

        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="accounts">帐号</param>
        /// <param name="password">密码【密文】</param>
        /// <returns>Code:1-成功;2-帐号不存在;3-密码错误</returns>
        public Response Verify(string username, string password)
        {
            Response _resp = new Response();
            var _user = base.Repository.Find(a => a.PhoneNumber == username);
            if (_user == null)
            {
                _resp.Code = 2;
                _resp.Message = "用户不存在";
            }
            else if (_user.Password == password)
            {
                _resp.Code = 1;
                _resp.Message = "验证通过";
            }
            else
            {
                _resp.Code = 3;
                _resp.Message = "帐号密码错误";
            }
            return _resp;
        }

        public int LoginUser(int i)
        {
            var _where = PredicateBuilder.New<User>();

            _where.And(p => DbFunctions.DiffDays(p.LastLoginTime, DateTime.Now) < 24 * i);
            if(i!=0)  _where.And(p => DbFunctions.DiffDays(p.LastLoginTime, DateTime.Now) >24 * (i-1));
            return this.FindList(_where).ToList().Count();
        }
        
      
    }
}

