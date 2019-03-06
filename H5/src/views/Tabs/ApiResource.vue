<template>
    <el-scrollbar wrap-class="scrollbar-wrapper-y">
        <div>
            <div class="flex">
                <!-- <span>选择系统 :</span>
                <el-select size="mini" v-model='systemSelect' placeholder="请选择系统">
                        <el-option label="区域一" value="shanghai"></el-option>
                        <el-option label="区域二" value="beijing"></el-option>
                </el-select> -->
                <span>资源名称:</span>
                <el-input v-model="keyword" size="mini" @keyup.enter.native="currentPage=1;flush();" placeholder="输入关键字搜索"/>
                <el-button type="primary" size="mini" @click='currentPage=1;flush();'>查询</el-button>
                <div class="flex1"></div>
                <el-button type="success" size="mini" icon='el-icon-circle-plus' @click='edit(0)'>创建资源</el-button>
            </div>
            <el-table
                    ref="multipleTable"
                    :data="items"
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
                        width="80"
                        align='center'
                        label="序号">
                    <template slot-scope="scope">
                        {{(currentPage - 1 ) * pageSize + scope.$index  + 1}}
                    </template>
                </el-table-column>
                <el-table-column
                        prop="name"
                        label="名称"
                        align='center'
                >
                </el-table-column>
                <el-table-column
                        prop="displayName"
                        label="描述"
                        align='center'
                >
                </el-table-column>
               
                <el-table-column
                        label="操作"
                        align='center'
                        width='240'
                >
                    <template slot-scope="scope">
                        <el-button type="primary" size="mini" @click='edit(1,scope.row)'>编辑</el-button>
                        <el-button type="warning" size="mini" @click='enable(scope.row)'>{{scope.row.enabled|enable}}</el-button>
                        <!-- <el-button @click="roleCheck(scope.row)" type="text" size="small">模块管理</el-button> -->
                    </template>
                </el-table-column>
            </el-table>
            <el-pagination
                            class='page_footer_box'
                            @size-change="currentPage=1;flush();"
                            @current-change="flush"
                            :current-page.sync="currentPage"
                            :page-sizes="[10, 20, 30, 50]"
                            :page-size.sync="pageSize"
                            layout="total, sizes, prev, pager, next, jumper"
                            :total="total">
            </el-pagination>
            
        </div>
        <ApiResourceEdit :config="config" @close="flush" ></ApiResourceEdit>
    </el-scrollbar>
</template>

<script>
    import ApiResourceEdit from '../../components/dialog/ApiResourceEdit'

    export default {
        components: {
            ApiResourceEdit,
        },
        data() {
            return {
                items: [],
                keyword: '',
                systemSelect: '',
                show: false,
                // 分页
                currentPage: 1,
                pageSize:10,
                total:0,
                config:{
                    show:false,
                    width:500,
                    title:"",
                    type:0,//0 创建，1 修改名称
                    data:{}
                }
            }
        },
        filters:{
            enable(value){
                if(value) return "停用";
                else return "启用";
            }
        },
        mounted() {
            this.flush();
        },
        methods: {
            async flush() {
                let result = await this.$http.post("/base/api/ApiResource/Query", {"pageIndex": this.currentPage, "pageSize": this.pageSize,name:this.keyword});
                this.items = result.list;
                this.total = result.totalCount;
            },
            handleSelectionChange(val) {
                this.selectitems = val;
            },
            roleCheck(row) {
                console.log(row);
            },
            // 资源编辑
            edit(type,row) {
                this.config.type = type;
                switch(type){
                    case 0:
                        this.config.title='创建资源';this.config.data = {};break;
                    case 1:
                        this.config.title='编辑资源';this.config.data = {name:row.name,displayName:row.displayName};break;
                }
                this.config.show = true;
            },
            async enable(row){
                await this.$http.post(`/base/api/ApiResource/Enabled?name=${row.name}`);
                this.flush();
            }
        }
    }
</script>

<style lang='scss' scoped>
    .page_footer_box {
        float: right;
        margin: 3px 10px;
    }
</style>