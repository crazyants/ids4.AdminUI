<template>
   <div>
       <h1 class='title'>admin</h1>
       <svg
               :class="{'is-active':isActive}"
               class="hamburger"
               viewBox="0 0 1024 1024"
               xmlns="http://www.w3.org/2000/svg"
               width="64"
               height="64"
               @click="toggleSideBar">
           <path d="M408 442h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8zm-8 204c0 4.4 3.6 8 8 8h480c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8H408c-4.4 0-8 3.6-8 8v56zm504-486H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zm0 632H120c-4.4 0-8 3.6-8 8v56c0 4.4 3.6 8 8 8h784c4.4 0 8-3.6 8-8v-56c0-4.4-3.6-8-8-8zM142.4 642.1L298.7 519a8.84 8.84 0 0 0 0-13.9L142.4 381.9c-5.8-4.6-14.4-.5-14.4 6.9v246.3a8.9 8.9 0 0 0 14.4 7z" />
       </svg>
   </div>
</template>

<script>

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
    .hamburger {
         display: inline-block;
         cursor: pointer;
         width: 36px;
         height: 36px;
         vertical-align: middle;
         fill: #dbdbdb;
     }
    .hamburger.is-active {
        transform: rotate(180deg);
    }
</style>