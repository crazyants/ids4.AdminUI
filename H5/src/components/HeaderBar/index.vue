<template>
   <div>
       <h1 class='title'>admin</h1>
       <toggleAside :toggle-side-bar="toggleSideBar" :is-active="isActive" class='togole_bar'></toggleAside>
   </div>
</template>

<script>
    import toggleAside from './toggleAside/index'

    export default {
        name: "home",
        props: {
            isActive: {
                type: Boolean,
                default: false
            },
            toggleSideBar: {
                type: Function,
                default: null
            }
        },
        components: {
            toggleAside,
        },
        data: function () {
            return {};
        },

        mounted() {

        },
        methods: {

        }
    };
</script>

<style scoped>
    .title {
        margin: 0;
        color: #fff;
        float: left;
        width: 200px;
    }
    .togole_bar {
        float: left;

    }
</style>