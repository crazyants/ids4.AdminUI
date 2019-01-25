<template>
    <div>
        参数值：{{name}}
    </div>
</template>

<script>
    export default {
        props:['name'],
      data() {
          return {
              val: ""
          }
      },
      mounted(){
          this.val = this.$route.params;
      }
    }
</script>

<style scoped>

</style>