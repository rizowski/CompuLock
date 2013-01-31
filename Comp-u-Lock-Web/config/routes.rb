CompULockWeb::Application.routes.draw do
  root :to => "home#index"
  get "computer_accounts/index"

  get "computer_accounts/edit"

  get "computers/index"

  get "computers/edit"

  get "account/index"

  get "account/edit"

  get "home/index"
  
  devise_for :users
  devise_scope :user do 
    match '/users/sign_out' => 'devise/sessions#destroy', :as => :destroy_user_session
  end
  match '/users/index', :as => :user_index
  match '/users/list', :as => :user_list

  match ':controller(/:action(/:id))(.:format)'
end
