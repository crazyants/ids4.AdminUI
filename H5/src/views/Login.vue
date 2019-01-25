<template>
  <el-form
    :model="logindata"
    status-icon
    :rules="loginrules"
    ref="loginForm"
    label-width="100px"
    class="login-ruleForm"
  >
    <el-form-item label="用户名" prop="userName">
      <el-input v-model.number="logindata.userName"></el-input>
    </el-form-item>
    <el-form-item label="密码" prop="passWord">
      <el-input type="password" v-model="logindata.passWord" autocomplete="off"></el-input>
    </el-form-item>
    <el-form-item label="记住我">
      <el-switch v-model="logindata.rember"></el-switch>
    </el-form-item>
    <el-form-item>
      <el-button type="primary" @click="submitForm('loginForm')">提交</el-button>
      <el-button @click="resetForm('loginForm')">重置</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
export default {
  name: "login",
  data() {
    return {
      logindata: {
        userName: "superadmin",
        passWord: "666666",
        rember : false
      },
      loginrules: {
        userName: [
          { required: true, message: "用户名不能为空" }
          // { validator: validatePass, trigger: 'blur' }
        ],
        passWord: [
          { required: true, message: "密码不能为空" } //,trigger: 'blur'
        ]
      }
    };
  },
  methods: {
    submitForm(formName) {
      this.$refs[formName].validate(valid => {
        if (valid) {
          this.login();
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    resetForm(formName) {
      this.$refs[formName].resetFields();
    },
    async login() {
      const data = new FormData();
      data.append("grant_type", "password");
      data.append("client_id", "ro.client");
      data.append("client_secret", "secret");
      data.append("username", this.logindata.userName);
      data.append("password", this.logindata.passWord);
      const config = {
        headers: {
          "Content-Type": "multipart/form-data"
        }
      };
      const result = await this.$http.post("/connect/token", data, config);
      this.$http.setheader(result.access_token);
      this.$router.push("/home/test");
    }
  }
};
</script>

<style scoped>
</style>