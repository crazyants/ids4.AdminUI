<template>
    <dialogBar @on-close="close" v-if="config.show" :dialog-width="config.width">
        <div slot="header">{{config.title}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-form ref="form" :model="config.data" :rules="rules" label-width="80px">
                <el-form-item label="权限名称" prop='name'>
                    <el-input size='mini' v-model="config.data.name"></el-input>
                </el-form-item>
                <el-form-item label="Controller" prop='controllerName'>
                    <el-input size='mini' v-model="config.data.controllerName"></el-input>
                </el-form-item>
                <el-form-item label="Action" prop='actionName'>
                    <el-input size='mini' v-model="config.data.actionName"></el-input>
                </el-form-item>
                <el-form-item label="接口地址" prop='url'>
                    <el-input size='mini' v-model="config.data.url"></el-input>
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
                    type:0,//0 创建，1 修改名称
                    data:{}
                }
            }
        },
        data() {
            return {
                rules: {
                    name: [
                        {required: true, message: '请输入权限名称', trigger: 'blur'},
                    ],
                    controllerName: [
                        {required: true, message: '请输入控制器名称', trigger: 'blur'},
                        { min: 1, max: 20, message: '长度在 1 到 20 个字符', trigger: 'blur' }
                    ],
                    actionName: [
                        {required: true, message: '请输入Action名称', trigger: 'blur'},
                    ],
                    url: [
                        {required: true, message: '请输入接口地址', trigger: 'blur'},
                    ]
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
                         switch(this.config.type){
                            case 0:
                                await this.$http.post("/base/api/App/AddPermission", this.config.data);break;
                            case 1:
                                await this.$http.post("/base/api/App/UpdatePermission", this.config.data);break;
                        }
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