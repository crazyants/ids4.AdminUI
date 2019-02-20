<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <span>选择系统 :</span>
                <el-select size="mini" v-model='systemSelect' placeholder="请选择系统">
                        <el-option label="区域一" value="shanghai"></el-option>
                        <el-option label="区域二" value="beijing"></el-option>
                    </el-select>
                <span>关键字 :</span>
                <el-input v-model="keyword" size="mini" placeholder="输入关键字搜索"/>
                <div style="width:20px"></div>
                <el-button type="primary" size="mini" @click='flush'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='eduitNameRole("")'>创建角色</el-button>
                <el-button type="danger" size="mini" icon='el-icon-delete' @click='delRole'>删除角色</el-button>
                <div style="width:20px"></div>
            </div>
            <el-table
                    ref="multipleTable"
                    :data="roleData"
                    tooltip-effect="dark"
                    border
                    align='center'
                    :stripe='true'
                    size='mini'
                    style="width: 100%"
                    @selection-change="handleSelectionChange">
                <el-table-column
                        type="selection"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        type="index"
                        width="50"
                        align='center'
                        label="序号">
                </el-table-column>
                <el-table-column
                        prop="id"
                        label="权限ID"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="name"
                        label="角色名称"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="createTime"
                        label="创建时间"
                        align='center'
                        show-overflow-tooltip>
                </el-table-column>
                <el-table-column
                        label="操作"
                        align='center'
                        width='160'
                >
                    <template slot-scope="scope">
                        <el-button type="text" size="small" @click='eduitNameRole(scope.row)'>编辑</el-button>
                        <el-button @click="roleCheck(scope.row)" type="text" size="small">权限组分配</el-button>
                    </template>
                </el-table-column>
            </el-table>
            <el-row class='page_footer'>
                <el-col :span='24'>
                    <el-pagination
                            class='page_footer_box'
                            @size-change="flush"
                            @current-change="flush"
                            :current-page.sync="currentPage"
                            :page-sizes="[10, 20, 30, 50]"
                            :page-size.sync="pageSize"
                            layout="total, sizes, prev, pager, next, jumper"
                            :total="total">
                    </el-pagination>
                </el-col>
            </el-row>
        </div>
        <RoleEduit :is-show.sync='show' @close-dailog="RoleEduitDilagHide" :dialog-tittle='dialogTittle'
                   :role-info='roleInfo'></RoleEduit>

        <RoleNameEduit :is-show.sync='RoleNameEduitShow' @close-dailog="RoleEduitDilagHide" :dialog-tittle='RoleNameEduitTitle'
                   :role-info='roleInfo'></RoleNameEduit>
    </el-scrollbar>
</template>

<script>
    import RoleEduit from '../../components/dialog/RoleEduit'
    import RoleNameEduit from '../../components/dialog/RoleNameEduit'

    export default {

        components: {
            'RoleEduit': RoleEduit,
            'RoleNameEduit': RoleNameEduit,
        },
        data() {
            return {
                roleData: [],
                multipleSelection: [],
                keyword: '',
                systemSelect: '',
                // 对话框
                dialogTittle: '',
                show: false,
                roleInfo: {},
                // 角色名称编辑
                RoleNameEduitShow:false,
                RoleNameEduitTitle:'',
                // 分页
                currentPage: 1,
                pageSize:10,
                total:0
            }
        },
        mounted() {
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/Role/Query", {"pageIndex": this.currentPage, "pageSize": this.pageSize,name:this.keyword});
                this.roleData = result.list;
                this.total = result.totalCount;
            },
            handleSelectionChange(val) {
                this.multipleSelection = val;
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
            roleCheck(row) {
                console.log(row);
            },

            // 角色名编辑
            eduitNameRole(row) {
                if(row){
                    this.RoleNameEduitTitle='编辑角色名称';
                    this.roleInfo = row
                }else {
                    this.RoleNameEduitTitle='创建角色名称';
                    this.roleInfo = {}
                }
                this.RoleNameEduitShow = true
            },

            RoleEduitDilagHide() {
                this.roleInfo = {}
                this.show = false
            }
        }
    }
</script>

<style lang='scss' scoped>
   


    .page_footer {
        margin-top: 10px;
    }

    .page_footer_box {
        float: right;
    }
</style>