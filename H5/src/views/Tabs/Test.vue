<template>
    <div>
<input type="text" value="测试">
<button @click="reflush()"> 刷新</button>
<button @click="to()"> 跳转</button>
    </div>
</template>

<script>
import { mapActions } from "vuex";
    export default {
        methods:{
            ...mapActions("tab",["reflush"]),
            to(){
                var val = new Date().getTime();
                this.$router.push( '/home/hasparam/'+val);
            }
        },
        inject:['reload']
    }
</script>

<style scoped>

</style>