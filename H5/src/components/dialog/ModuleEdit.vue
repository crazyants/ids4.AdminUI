<template>
    <dialogBar @on-close="close" v-if="config.show" :dialog-width="config.width">
        <div slot="header">{{config.title}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-form ref="form" :model="config.data" :rules="rules" label-width="80px">
                <el-form-item label="编号" prop='code'>
                    <el-input size='mini' v-model="config.data.code"></el-input>
                </el-form-item>
                <el-form-item label="模块名称" prop='name'>
                    <el-input size='mini' v-model="config.data.name"></el-input>
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
                    code: [
                        {required: true, message: '请输入模块编号', trigger: 'blur'},
                    ],
                    name: [
                        {required: true, message: '请输入模块名称', trigger: 'blur'},
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
                                await this.$http.post("/base/api/App/AddModule", this.config.data);break;
                            case 1:
                                await this.$http.post("/base/api/App/UpdateModule", this.config.data);break;
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