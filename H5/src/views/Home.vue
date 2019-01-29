<template>
    <el-container id="content_box">
        <el-header>
            <HeaderBar :toggle-side-bar="toggleSideBar" :is-active="isCollapse" > </HeaderBar>
        </el-header>
        <el-container class='aside_box'>
            <el-scrollbar wrap-class="scrollbar-wrapper">
                <el-aside width="auto">
                    <el-menu
                            default-active="1-4-1"
                            class="el-menu-vertical-demo"
                            @open="handleOpen"
                            @close="handleClose"
                            :collapse="isCollapse"
                            :collapse-transition="false"
                            background-color="#222d32"
                            text-color="#fff"
                            :unique-opened='true'
                    >

                        <el-submenu index="1">
                            <template slot="title">
                                <i class="el-icon-location"></i>
                                <span slot="title">导航一</span>
                            </template>
                            <el-menu-item index="1-1-1">选项1</el-menu-item>
                            <el-menu-item index="1-1-2">选项2</el-menu-item>
                            <el-menu-item index="1-1-3">选项3</el-menu-item>
                            <el-menu-item index="1-1-4">选项4</el-menu-item>
                            <el-menu-item index="1-1-5">选项5</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-7">选项7</el-menu-item>
                        </el-submenu>
                        <el-menu-item index="2">
                            <i class="el-icon-menu"></i>
                            <span slot="title">导航二</span>
                        </el-menu-item>
                        <el-menu-item index="3" disabled>
                            <i class="el-icon-document"></i>
                            <span slot="title">导航三</span>
                        </el-menu-item>
                        <el-menu-item index="4">
                            <i class="el-icon-setting"></i>
                            <span slot="title">导航四</span>
                        </el-menu-item>

                        <el-submenu index="1-4">
                            <template slot="title">
                                <i class="el-icon-location"></i>
                                <span slot="title">导航无</span>
                            </template>
                            <el-menu-item index="1-1-1">选项1</el-menu-item>
                            <el-menu-item index="1-1-2">选项2</el-menu-item>
                            <el-menu-item index="1-1-3">选项3</el-menu-item>
                            <el-menu-item index="1-1-4">选项4</el-menu-item>
                            <el-menu-item index="1-1-5">选项5</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-6">选项6</el-menu-item>
                            <el-menu-item index="1-1-7">选项7</el-menu-item>
                        </el-submenu>
                    </el-menu>
                </el-aside>
            </el-scrollbar>
            <el-main>
                <tab-view/>
            </el-main>
        </el-container>
    </el-container>
</template>

<script>
    // @ is an alias to /src
    import TabView from '../components/TabView.vue'
    import HeaderBar from '../components/HeaderBar/index'
    export default {
        name: "home",
        data: function () {
            return {
                isCollapse: false,
                mainboxH: 0
            };
        },
        components: {
            TabView,
            HeaderBar
        },
        mounted() {

        },
        methods: {
            handleOpen(key, keyPath) {
                console.log(key, keyPath);
            },
            toggleSideBar() {
                this.isCollapse = !this.isCollapse
            },
            handleClose(key, keyPath) {
                console.log(key, keyPath);
            }
        }
    };
</script>

<style type='scss' scoped>
    #content_box {
        height: 100%;
    }
    .el-header {
        background-color: #337ab7;
        color: #333;
        line-height: 50px;
        height: 50px !important;
        padding: 0;
    }
    .aside_box {
        height: 100%;
        background-color: #222d32;
        color: #333;
    }

    .el-menu-vertical-demo:not(.el-menu--collapse) {
        width: 200px;
        min-height: 100%;
    }
    .el-main {
        background-color: #fff;
        color: #333;
        height: 100%;
        padding: 0;
    }
</style>