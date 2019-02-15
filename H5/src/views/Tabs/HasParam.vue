<template>
    <div>
        参数值：{{val}} 当前时间：{{curtime}}
    </div>
</template>

<script>
    export default {
        props:['name'],
      data() {
          return {
              val: "",
              curtime:null
          }
      },
      mounted(){
          this.val = this.$route.params.name;
          this.curtime = new Date().getTime();
        //   this.$nextTick(()=>{
        //     this.$store.commit("tab/ModifyTile", {fullPath:"/home/hasparam/"+this.val,title:`详细(${this.curtime})`});
        //   })
      }
    }
</script>

<style scoped>

</style>