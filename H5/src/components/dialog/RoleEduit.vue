<template>
    <dialogBar @on-close="closeMyself" v-if="isShow" :dialog-width="dialogWidth">
        <div slot="header">{{dialogTittle}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-form ref="roleForm" :model="roleForm" :rules="rules" label-width="80px">
                <el-form-item label="日期" prop='data'>
                    <el-col :span="18">
                        <el-date-picker type="date" placeholder="选择日期" v-model="roleForm.date" style="width: 100%;"></el-date-picker>
                    </el-col>
                </el-form-item>
                <el-form-item label="姓名" prop='name'>
                    <el-input v-model="roleForm.name"></el-input>
                </el-form-item>
                <el-form-item label="地址" prop='desc'>
                    <el-input type="textarea" v-model="roleForm.desc"></el-input>
                </el-form-item>
            </el-form>
        </div>
        <div slot="footer">
            <el-button type="info" size='mini' @click='closeMyself'>取消</el-button>
            <el-button type="primary" size='mini' @click='onSubmit("roleForm")'>确认</el-button>
        </div>


    </dialogBar>
</template>
<script>
    import dialogBar from './comment/dialogBar'
    export default {
        components:{
            'dialogBar': dialogBar,
        },
        props: {
            isShow: {
                //弹窗组件是否显示 默认不显示
                type: Boolean,
                default: false,
                required:true, //必须
            },
            dialogWidth:{
                type: Number,
                default: 500,
            },
            dialogTittle:{
                type: String
            },
            roleInfo:{
                type:Object
            }
        },
        data () {
            return{
                roleForm:{
                    date:'',
                    name:'',
                    desc:'',
                },
                rules:{
                    name:[
                        {required: true,message:'请输入活动名字',trigger:'blur'},
                    ],
                    date:[
                        { required:true,message:'请选择日期',trigger:'change'}
                    ],
                    desc:[
                        {required:true,message:"请输入地址",trigger:'blur'}
                    ]
                }
            }
        },
        created(){
            this.roleForm = this.roleInfo
        },
        methods: {
            closeMyself() {
                this.$refs.roleForm.resetFields();
                //this.$emit("close-dailog");
                this.$emit('update:isShow', false)
                //如果需要传参的话，可以在"on-close"后面再加参数，然后在父组件的函数里接收就可以了。
            },
            onSubmit(roleForm){
                this.$refs[roleForm].validate( (valid) => {
                    if(valid){
                        alert('sbumit!')
                    }else {
                        console.log('error')
                    }
                })
            }
        }
    }
</script>
<style>

</style>