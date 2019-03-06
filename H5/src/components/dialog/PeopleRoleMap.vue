<template>
    <dialogBar @on-close="close" v-if="config.show" :dialog-width="config.width">
        <div slot="header">{{config.title}}</div>
        <div class="dialog_publish_main" slot="main">
            <el-transfer
    v-model="selects"
    :props="{
      key: 'id',
      label: 'name'
    }"
    :data="list">
    </el-transfer>
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
                    data:{}
                }
            }
        },
        data() {
            return {
                list:[],
                selects:[],
                rules: {
                    name: [
                        {required: true, message: '请输入角色名称', trigger: 'blur'},
                    ],
                }
            }
        },
        methods: {
            async initdata(userid){
                this.useid=userid;
                this.list.splice(0,this.list.length);
                this.selects.splice(0,this.selects.length);
                const httpresults = await this.$http.awaitTasks([
                    this.$http.post("/base/api/Role/Query", {"pageIndex": 1, "pageSize": 999}),
                    this.$http.post(`/base/api/User/Permission?userid=${userid}`)
                ]);
                this.list.push(...httpresults[0].list);
                this.selects.push(...httpresults[1]);
                console.log(httpresults);
            },
            close(result) {
                this.config.show = false;
                if(result) this.$emit("close");
            },
            async onSubmit() {
                console.log(this.selects)
                await this.$http.post(`/base/api/User/SetRole?userid=${this.useid}`, this.selects);
                this.close(true);
            }
        }
    }
</script>
<style lang="scss" scoped>
    .dialog_publish_main{
        padding-bottom: 10px;
    }
</style>