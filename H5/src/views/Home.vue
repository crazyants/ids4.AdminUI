<template>
    <el-container id="content_box">
        <el-scrollbar wrap-class="scrollbar-wrapper" class='aside_scrollbar_box'>
            <el-card class='antor_photo' :body-style="{ padding: ' 20px 0' }" v-if='!isCollapse'>
                <img src="../assets/images/timg.jpg" class="image">
                <p class='antor_name'>name</p>
            </el-card>
            <el-aside width="auto">
                <el-menu
                        default-active="1-4-1"
                        class="el-menu-vertical-demo"
                        @open="handleOpen"
                        @close="handleClose"
                        background-color="#2f4050"
                        text-color="#fff"
                        :collapse="isCollapse"
                        :collapse-transition="false"
                >
                    <el-submenu index="1">
                        <template slot="title">
                            <i class="el-icon-location"></i>
                            <span slot="title">导航一</span>
                        </template>
                        <el-menu-item-group>
                            <span slot="title">分组一</span>
                            <el-menu-item index="1-1">选项1</el-menu-item>
                            <el-menu-item index="1-2">选项2</el-menu-item>
                        </el-menu-item-group>
                        <el-menu-item-group title="分组2">
                            <el-menu-item index="1-3">选项3</el-menu-item>
                        </el-menu-item-group>
                        <el-submenu index="1-4">
                            <span slot="title">选项4</span>
                            <el-menu-item index="1-4-1">选项1</el-menu-item>
                        </el-submenu>
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
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                    <el-menu-item index="4">
                        <i class="el-icon-setting"></i>
                        <span slot="title">导航四</span>
                    </el-menu-item>
                </el-menu>
            </el-aside>
        </el-scrollbar>
        <el-container class='aside_box'>
            <el-header>
                <HeaderBar :toggle-side-bar="toggleSideBar" :is-active="isCollapse"></HeaderBar>
            </el-header>
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

<style lang='scss' scoped>
    #content_box {
        height: 100%;
    }

    .el-header {
        background-color: #b3c0d1;
        color: #333;
        line-height: 60px;
    }

    .aside_box {
        height: 100%;
        background-color: #d3dce6;
        color: #333;
    }

    .el-menu-vertical-demo:not(.el-menu--collapse) {
        width: 200px;
        min-height: 100%;
    }

    .el-main {
        background-color: #e9eef3;
        color: #333;
        height: 100%;
    }

    .image {
        width: 60px;
        height: 60px;
        border-radius: 30px;
        display: inline-block;
    }

    .antor_photo {
        text-align: center;
        background-color: #2f4050;
        border-radius: 0;
        border: none;
        border-right: 1px solid #fff;
        .el-card__body {
            padding: 0 10px;
        }
        .antor_name {
            color: #fff;
            margin-top: 6px;
        }
    }

</style>