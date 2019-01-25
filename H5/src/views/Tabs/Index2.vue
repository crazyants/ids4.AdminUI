<template>
    <div>
<input type="text" value="测试">
<button @click="reflush()"> 刷新</button>
    </div>
</template>

<script>
    export default {
        methods:{
            reflush(){
                this.$router.push("/home/test?sdaa=1231")
            }
        }
    }
</script>

<style scoped>

</style>