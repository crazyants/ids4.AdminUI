<template>
    <dialogBar @on-close="close" v-if="config.show" :dialog-width="config.width">
        <div slot="header">{{config.title}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-form ref="form" :model="config.data" :rules="rules" label-width="80px">
                <el-form-item label="角色名称" prop='name'>
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
                    data:{}
                }
            }
        },
        data() {
            return {
                rules: {
                    name: [
                        {required: true, message: '请输入活动名字', trigger: 'blur'},
                    ],
                }
            }
        },
        methods: {
            close(result) {
                this.$refs.form.resetFields();
                this.config.show = false;
                if(result) this.$emit("close");
            },
            onSubmit() {
                this.$refs.form.validate((valid) => {
                    if (valid) {
                       this.close(true);
                    }
                })
            }
        }
    }
</script>
<style>

</style>