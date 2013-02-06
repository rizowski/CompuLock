CompULockWeb::Application.routes.draw do
  root :to => "home#index"
  
  resource :home, :only => [:index]
  resource :user
  resource :computer, controller: :computer
  resources :account
  # resources :account_history
  # resources :account_process
  # resources :account_program
  
  namespace :api, defaults:{format:'json'} do
    namespace :v1  do
      resources :tokens, only: [:create, :destroy] # done
      resources :computers, only: [:index, :show, :create, :update, :destroy]
      resources :users, only: [:index, :update] # done
      resources :accounts, only: [:index, :create, :update, :show, :destroy] # Update
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
