<template>
    <dialogBar @on-close="close" v-if="config.show" :dialog-width="config.width">
        <div slot="header">{{config.title}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-form ref="form" :model="config.data" :rules="rules" label-width="80px">
                <el-form-item v-if="config.type==0" label="账号" prop='account'>
                    <el-input size='mini' v-model="config.data.account"></el-input>
                </el-form-item>
                <el-form-item v-if="config.type!=2" label="用户名称" prop='name'>
                    <el-input size='mini' v-model="config.data.name"></el-input>
                </el-form-item>
                <el-form-item v-if="config.type!=1" label="密码" prop='pwd'>
                    <el-input type="password" size='mini' v-model="config.data.pwd"></el-input>
                </el-form-item>
            </el-form>
        </div>
        <div slot="footer">
            <el-button type="info" size='mini' @click='close(false)'>取消</el-button>
            <el-button type="primary" size='mini' @click='onSubmit()'>确认</el-button>
        </div>
    </dialogBar>
</template>
<script>
    import dialogBar from './comment/dialogBar'
import { promised } from 'q';

    export default {
        components: {
            'dialogBar': dialogBar,
        },
        props: {
            config:{
                type: Object,
                default:{
                    show:false,
                    width:800,
                    title:"",
                    type:0,//0 创建，1 修改名称，2 重置密码
                    data:{}
                }
            }
        },
        data() {
            return {
                rules: {
                    name: [
                        {required: true, message: '请输入名称', trigger: 'blur'}
                    ],
                    account: [
                        {required: true, message: '请输入账号', trigger: 'blur'}
                    ],
                    pwd: [
                        {required: true, message: '请输入密码', trigger: 'blur'}
                    ],
                }
            }
        },
        methods: {
            close(result) {
                this.config.show = false;
                if(result) this.$emit("close");
            },
            onSubmit() {
                this.$refs.form.validate(async(valid) => {
                    if (valid) {
                       if(this.config.data.id) await this.$http.post("/base/api/User/UpdateName", this.config.data);
                       else await this.$http.post("/base/api/User/Create", this.config.data);
                       this.close(true);
                    }
                })
            }
        }
    }
</script>
<style lang="scss" scoped>
    .dialog_publish_main{
        padding-bottom: 10px;
    }
</style>