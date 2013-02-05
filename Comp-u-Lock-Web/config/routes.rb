CompULockWeb::Application.routes.draw do
  root :to => "home#index"
  resource :user
  resource :home
  resource :account do 
    resource :account_history
    resource :account_process
    resource :account_program
  end
  resource :computer
  
  namespace :api do
    namespace :v1  do
      resources :tokens, :only => [:create, :destroy]
      resources :computers
      resources :users, :only => [:show, :edit, :update]
    end
  end

  get "computer_accounts/index"

  get "computer_accounts/edit"

  get "computers/index"

  get "computers/edit"

  get "account/index"

  get "account/edit"

  get "home/index"
  
  devise_for :users
  devise_scope :user do 
    match '/users/sign_in' => 'devise/session#create', :as => :new_user_session
    match '/users/sign_out' => 'devise/sessions#destroy', :as => :destroy_user_session
  end

  match '/users/index', :as => :user_index
  match '/users/list', :as => :user_list
  match '/users/edit', :as => :user_edit

  match ':controller(/:action(/:id))(.:format)'
end
