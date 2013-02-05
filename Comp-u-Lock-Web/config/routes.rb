CompULockWeb::Application.routes.draw do
  root :to => "home#index"
  
  resource :home, :only => [:index]
  resource :user
  resources :computer
  resources :account
  resources :account_history
  resources :account_process
  resources :account_program
  
  namespace :api do
    namespace :v1  do
      resources :tokens, :only => [:create, :destroy]
      resources :computers
      resources :users, :only => [:index, :edit, :update]
      resources :accounts
    end
  end

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
