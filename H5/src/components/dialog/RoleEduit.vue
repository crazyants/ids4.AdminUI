<template>
    <dialogBar @on-close="closeMyself" v-if="isShow" :dialog-w="dialogW">
        <div slot="header">标题</div>
        <div class="dialog_publish_main" slot="main">
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
            <p>这里是内容插入到子组件的slot的name为main里面，可以在父组件中添加class定义样式，事件类型等各种操作</p>
        </div>
        <div slot="footer">
            <el-button type="success" size='mini' @click='closeMyself'>成功按钮</el-button>
            <el-button type="info" size='mini' @click='closeMyself'>信息按钮</el-button>
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
            dialogW:{
                type: Number,
                default: 500,
            }
        },
        data () {
            return{

            }
        },
        methods: {
            closeMyself() {
                this.$emit("close-dailog");
                //如果需要传参的话，可以在"on-close"后面再加参数，然后在父组件的函数里接收就可以了。
            },
        }
    }
</script>
<style>

</style>