<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
         <div style="height:20px;"></div>
            <el-form ref="form" status-icon :model="data" :rules="rules" label-width="150px">
                <el-form-item label="客户端id" prop='clientId'>
                    <el-input size='mini' v-model="data.clientId"></el-input>
                </el-form-item>
                <el-form-item label="客户端名称" prop='clientName'>
                    <el-input size='mini' v-model="data.clientName"></el-input>
                </el-form-item>
                <el-form-item label="授权类型" prop='allowedGrantTypes'>
                    <el-checkbox-group v-model="data.allowedGrantTypes">
                        <el-checkbox label="client_credentials" name="allowedGrantTypes"></el-checkbox>
                        <el-checkbox label="password" name="allowedGrantTypes"></el-checkbox>
                        <el-checkbox label="implicit" name="allowedGrantTypes"></el-checkbox>
                        <el-checkbox label="hybrid" name="allowedGrantTypes"></el-checkbox>
                    </el-checkbox-group>
                </el-form-item>
                <el-form-item label="访问token">
                    <el-switch v-model="data.allowAccessTokensViaBrowser"></el-switch>
                </el-form-item>
                <el-form-item label="登入跳转" prop='redirectUris'>
                    <el-input size='mini' v-model="data.redirectUris[0]"></el-input>
                </el-form-item>
                <el-form-item label="登出跳转" prop='postLogoutRedirectUris'>
                    <el-input size='mini' v-model="data.postLogoutRedirectUris[0]"></el-input>
                </el-form-item>
                <el-form-item label="设置跨域" prop='allowedCorsOrigins'>
                    <el-input size='mini' v-model="data.allowedCorsOrigins[0]"></el-input>
                </el-form-item>
                <el-form-item label="访问资源" prop='allowedScopes'>
                    <el-checkbox-group v-model="data.allowedScopes">
                        <el-checkbox label="openid" name="allowedScopes"></el-checkbox>
                        <el-checkbox label="profile" name="allowedScopes"></el-checkbox>
                        <el-checkbox v-for="resource in apiresources" :key="resource" :label="resource" name="allowedScopes"></el-checkbox>
                    </el-checkbox-group>
                </el-form-item>
                <el-form-item>
                    <el-button type="info" size='mini' @click='close(false)'>取消</el-button>
                    <el-button type="primary" size='mini' @click='onSubmit()'>确认</el-button>
                </el-form-item>
            </el-form>
    </el-scrollbar>
</template>
<script>
    export default {
        components: {
        },
        data() {
            var getValidateStr =(err)=>{
                const com = this;
                return (rule, value, callback) => {
                    if(com.data.allowedGrantTypes.indexOf('implicit')==-1&&com.data.allowedGrantTypes.indexOf('hybrid')==-1) return callback();
                    if(value&&value.length&&value[0])  callback();
                    else  callback(new Error(err));
                };
            }
            return {
                data:{
                    clientId:"",
                    clientName:"",
                    allowedGrantTypes:[],
                    allowedScopes:[],
                    allowAccessTokensViaBrowser:false,
                    redirectUris:[""],
                    postLogoutRedirectUris:[""],
                    allowedCorsOrigins:[""]
                },
                apiresources:[],
                rules: {
                    clientId: [
                        {required: true, message: '客户端id不能为空！', trigger: 'blur'},
                    ],
                    allowedGrantTypes: [
                        {type: 'array',required: true, message: '至少选择一个授权类型！', trigger: 'blur'},
                    ],
                    allowedScopes: [
                        {type: 'array',required: true, message: '至少选择一个访问资源！', trigger: 'blur'},
                    ],
                    redirectUris: [
                        {validator: getValidateStr('登入跳转不能为空！'), trigger: 'blur'},
                    ],
                    postLogoutRedirectUris: [
                        {validator: getValidateStr('登出跳转不能为空！'), trigger: 'blur'},
                    ],
                }
            }
        },
        computed:{
            needredirectUris(){
                if(this.data.allowedGrantTypes.indexOf('implicit')!=-1) return true;
                if(this.data.allowedGrantTypes.indexOf('hybrid')!=-1) return true;
                return false;
            }
        },
        mounted(){
            this.$http.post("/base/api/ApiResource/Query", {"pageIndex": 1, "pageSize": 999}).then(result=>{
                this.apiresources.push(...result.list.map(i=>i.name)) ;
            });
            var query = this.$router.currentRoute.query;
            if(query.id){
                this.$http.get("/base/api/Client/Query?clientid="+query.id).then(client=>{
                    Object.keys(this.data).forEach(key=>{
                        this.data[key]=client[key];
                    })
                })
            }
        },
        methods: {
            close(result) {
                this.$store.commit("tab/closeAndTo", {fullPath:"/home/client"});
            },
            onSubmit() {
                this.$refs.form.validate(async(valid) => {
                    if(!valid) return;
                    if(this.$router.currentRoute.name=="home.client.add")
                        await  this.$http.post("/base/api/Client/Create",this.data);
                    else if(this.$router.currentRoute.name=="home.client.update")
                        await  this.$http.post("/base/api/Client/Update",this.data);
                    this.$store.commit("tab/closeAndTo", {fullPath:"/home/client",flush:"flush"});
                    console.log(this.data);
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
.el-input{
    width: 400px;
}
.el-checkbox-group{
    display: inline-block;
}
    .dialog_publish_main{
        padding-bottom: 10px;
    }
</style>