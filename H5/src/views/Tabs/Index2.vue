<template>
    <div>
<input type="text" value="测试">
<button @click="reflush()"> 刷新</button>
    </div>
</template>

<script>
import { mapActions } from "vuex";
    export default {
        methods:{
            ...mapActions("tab",["reflush"])
        },
        inject:['reload']
    }
</script>

<style scoped>

</style>