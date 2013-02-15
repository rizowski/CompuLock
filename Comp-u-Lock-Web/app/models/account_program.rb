class AccountProgram < ActiveRecord::Base
  	attr_accessible :account_id, :name, :open_count
  
  	validates :name, presence: true
  	validates :account_id, presence: true

	belongs_to :account
end
