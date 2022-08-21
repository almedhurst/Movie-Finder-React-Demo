import ApiUtil from "../utilities/apiUtil";


const CategoryService = {
    GetCategories: () => ApiUtil.get('Categories/GetCategories')
}

export default CategoryService;