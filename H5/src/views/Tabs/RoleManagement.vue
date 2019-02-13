<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
        <el-row class='role_title'>
            <el-col :span="2">
                <div>
                    <h3 class='role_title_text'>角色管理</h3>
                </div>
            </el-col>
            <el-col :span="8" :offset="14">
                <div>
                    <el-button type="success" size="mini" icon='el-icon-circle-plus'>创建角色</el-button>
                    <el-button type="warning" size="mini" icon="el-icon-edit" @click='EduitRole'>编辑</el-button>
                    <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除</el-button>
                    <el-button type="primary" size="mini" icon='el-icon-news'>分配权限</el-button>
                </div>
            </el-col>
        </el-row>
        <el-row class='role_system'>
            <el-col :span="2">
                <span class='role_system_select'>选择系统 :</span>
            </el-col>
            <el-col :span="3">
                <el-select size="mini" v-model='systemSelect' placeholder="请选择系统">
                    <el-option label="区域一" value="shanghai"></el-option>
                    <el-option label="区域二" value="beijing"></el-option>
                </el-select>

            </el-col>
            <el-col :span="4" :offset="14">
                <el-input
                        v-model="search"
                        size="mini"
                        placeholder="输入关键字搜索"/>
            </el-col>
        </el-row>
        <el-row>
            <el-col>
                <el-table
                        ref="multipleTable"
                        :data="tableData3.filter(data => !search || data.name.toLowerCase().includes(search.toLowerCase()))"
                        tooltip-effect="dark"
                        border
                        style="width: 100%"
                        @selection-change="handleSelectionChange">
                    <el-table-column
                            type="selection"
                            >
                    </el-table-column>
                    <el-table-column
                            label="日期"
                            >
                        <template slot-scope="scope">{{ scope.row.date }}</template>
                    </el-table-column>
                    <el-table-column
                            prop="name"
                            label="姓名"
                            >
                    </el-table-column>
                    <el-table-column
                            prop="address"
                            label="地址"
                            show-overflow-tooltip>
                    </el-table-column>
                    <el-table-column
                            label="操作"
                            width='100'
                    >
                        <template slot-scope="scope">
                            <el-button @click="handleClick(scope.row)" type="text" size="small">查看</el-button>
                            <el-button type="text" size="small">编辑</el-button>
                        </template>
                    </el-table-column>
                </el-table>
            </el-col>
        </el-row>
        </div>
        <RoleEduit :is-show='show' @close-dailog="RoleEduitHide"></RoleEduit>
    </el-scrollbar>
</template>

<script>
    import RoleEduit from '../../components/dialog/RoleEduit'
    export default {

        components:{
            'RoleEduit': RoleEduit,
        },
        data() {
            return {
                tableData3: [{
                    date: '2016-05-03',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-02',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-04',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-01',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-08',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-06',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }, {
                    date: '2016-05-07',
                    name: '王小虎',
                    address: '上海市普陀区金沙江路 1518 弄'
                }],
                multipleSelection: [],
                search: '',
                systemSelect: '',
                // 对话框
                show: false
            }
        },
        methods: {
            handleSelectionChange(val) {
                this.multipleSelection = val;
            },
            handleClick(row) {
                console.log(row);
            },
            delRole() {
                // if (this.multipleSelection.length < 1) {
                //     this.$message({
                //         message: '请选择需要删除的角色!',
                //         type: 'warning'
                //     });
                //     return
                // }
                this.$confirm('确认删除所选角色?', '删除', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.$message({
                        type: 'success',
                        message: '删除成功!'
                    });
                }).catch(() => {
                    this.$message({
                        type: 'info',
                        message: '已取消删除'
                    });
                });
            },
            EduitRole() {
                this.show = true
            },
            RoleEduitHide(){
                this.show = false
            }
        }
    }
</script>

<style lang='scss' scoped>
    .role_title {
        padding: 4px 0;
        border-bottom: 1px solid #ccc;
        .role_title_text {
            line-height: 28px;
            text-indent: 1em;
        }

    }

    .role_system {
        padding: 4px 0;
        border-bottom: 2px solid #e50a10;
        .role_system_select {
            line-height: 30px;
            display: inline-block;
            text-indent: 1.5em;
        }
    }
</style>